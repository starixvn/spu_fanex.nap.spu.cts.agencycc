namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.AntiFraudCenter;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Sentry;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface INormalAccountInsertAgencyJobService
    {
        Task<bool> Execute(int batchExternalSize, int batchInternalSize);
    }

    public class NormalAccountInsertAgencyJobService : INormalAccountInsertAgencyJobService
    {
        private readonly INormalAccountInsertAgencyService _service;
        private readonly ISentryClient _sentryClient;
        private readonly string _serviceName = "NormalAccountInsertAgency";
        private readonly IAntiFraudCenterServices _antiFraudCenterService;
        private readonly IAppSettings _appSettings;
        private readonly ICustClassToMainDBService _custClassToMainDBService;

        public NormalAccountInsertAgencyJobService(
            INormalAccountInsertAgencyService service,
            ISentryClient sentryClient,
            IAntiFraudCenterServices antiFraudCenter,
            IAppSettings appSettings,
            ICustClassToMainDBService custClassToMainDBService)
        {
            _service = service;
            _sentryClient = sentryClient;
            _antiFraudCenterService = antiFraudCenter;
            _appSettings = appSettings;
            _custClassToMainDBService = custClassToMainDBService;
        }

        public Task<bool> Execute(int batchExternalSize, int batchInternalSize)
        {
            try
            {
                var isContinue = true;

                if (batchInternalSize <= 0)
                {
                    batchInternalSize = 5000;
                }

                do
                {
                    isContinue = JobRun(batchExternalSize, batchInternalSize);
                }
                while (isContinue);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private bool JobRun(int batchExternalSize, int batchInternalSize)
        {
            var normalAccountModel = _service.GetNormalAccounts();

            if (normalAccountModel.IsContinute)
            {
                var failedThreads = new ConcurrentBag<bool>();

                var custChunks = normalAccountModel.NormalAccount.Chunk(batchInternalSize);

                custChunks
                    .AsParallel()
                    .WithDegreeOfParallelism(4)
                    .ForAll(custBatch => InsertNormalAccount(custBatch, batchExternalSize, failedThreads));

                _service.CompleteNormalAccountInsert(normalAccountModel.ScannedMaxId, failedThreads.IsEmpty);
            }

            return normalAccountModel.IsContinute;
        }

        private void InsertNormalAccount(IEnumerable<GetNormalAccountAgencyEntity> custs, int batchExternalSize, ConcurrentBag<bool> failedThreads)
        {
            var inserCustomers = custs.Select(x => new InsertNormalAccountAgencyEntity(x));

            var result = _service.InsertNormalAccount(inserCustomers, _serviceName, _appSettings.NormalAccountIsWriteLog);

            if (string.IsNullOrEmpty(result.ErrorMessage))
            {
                var custIDs = result.CustInfos.Select(x => x.CustID).Distinct();

                Parallel.Invoke((() =>
                {
                    var isPushCCSuccess = _custClassToMainDBService.PushExternalPackageByCustId(custIDs, new PushDataToExternalModel
                    {
                        RoleGroup = RoleGroupType.Agency,
                        ExternalBatchSize = batchExternalSize,
                        IsUpdateDangerToMainDB = false, // --> Not Update
                        IsWriteLog = _appSettings.NormalAccountIsWriteLog,
                        ServiceName = _serviceName
                    });

                    if (!isPushCCSuccess)
                    {
                        failedThreads.Add(false);
                    }
                }),
                () =>
                {
                    _antiFraudCenterService.DeleteAFCAccounts(custIDs, FromFlow.CTSServices, _appSettings.IsSyncAFCData, _appSettings.AFCDeleteAccountURL);
                });
            }
        }
    }
}
