namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    public interface IDailyNormalClassificationAgencyJobService
    {
        Task<bool> Execute();
    }

    public class DailyNormalClassificationAgencyJobService : IDailyNormalClassificationAgencyJobService
    {
        private readonly string _serviceName = "DailyNormalClassificationAgency";
        private readonly ISentryClient _sentryClient;
        private readonly IDailyNormalClassificationAgencyService _service;

        public DailyNormalClassificationAgencyJobService(ISentryClient sentryClient, IDailyNormalClassificationAgencyService service)
        {
            _sentryClient = sentryClient;
            _service = service;
        }

        public Task<bool> Execute()
        {
            try
            {
                JobRun();
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void JobRun()
        {
            bool isContinue;

            do
            {
                isContinue = _service.Start()
                                        .GetCustomerForDailyNormalScanning()
                                        .PreprocessDailyNormalCustomer()
                                        .UpdateLastScan();
            } while (isContinue);
        }
    }
}
