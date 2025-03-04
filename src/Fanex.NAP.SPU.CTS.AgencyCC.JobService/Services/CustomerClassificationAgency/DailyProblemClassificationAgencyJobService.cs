namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;
    using System.Linq;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;

    public interface IDailyProblemClassificationAgencyJobService
    {
        Task<bool> Execute(int batchSize);
    }

    public class DailyProblemClassificationAgencyJobService : IDailyProblemClassificationAgencyJobService
    {
        private readonly string _serviceName;
        private readonly ISentryClient _sentryClient;
        private readonly IDailyProblemClassificationAgencyService _service;
        private readonly IAppSettings _appSettings;

        public DailyProblemClassificationAgencyJobService(
            ISentryClient sentryClient,
            IDailyProblemClassificationAgencyService service,
            IAppSettings appSettings)
        {
            _sentryClient = sentryClient;
            _service = service;
            _appSettings = appSettings;

            _serviceName = "DailyProblemAccountWLClassificationAgency";
        }

        public Task<bool> Execute(int batchSize)
        {
            try
            {
                if (batchSize > 0)
                {
                    JobRun(batchSize);
                }
                else
                {
                    Utilities.LogSentryError(_serviceName, "InValid Batch size", (batchSize), new Exception(), _sentryClient);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void JobRun(int batchSize)
        {
            var paClassificationAgency =
                _service
                    .Start()
                    .GetPADailyAgencyScanFromQueue()
                    .GetProblemAccountLosingClassification();

            if (paClassificationAgency.IsNotNullOrEmpty())
            {
                foreach (var paBatch in paClassificationAgency.Chunk(batchSize))
                {
                    var isSuccess =
                        _service
                            .GetInstanceProblemWLClassificationAgencyService()
                            .InsertPAWLClassificationAgency(batchSize, paBatch)
                            .PushCCToExternal(new PushDataToExternalModel
                            {
                                RoleGroup = RoleGroupType.Agency,
                                ExternalBatchSize = batchSize,
                                IsUpdateDangerToMainDB = false, // not handle
                                IsWriteLog = _appSettings.ProblemAccountIsWriteLog,
                                ServiceName = _serviceName
                            });

                    if (!isSuccess)
                    {
                        return;
                    }
                }
            }

            var isContinue = _service.UpdateComplete();

            if (isContinue)
            {
                JobRun(batchSize);
            }
        }
    }
}
