namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.DataLogTracking;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Sentry;
    using System;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface INewCustomerClassificationAgencyJobService
    {
        Task<bool> Execute(int batchSize);
    }

    public class NewCustomerClassificationAgencyJobService : INewCustomerClassificationAgencyJobService
    {
        private readonly string _serviceName = "NewCTSCustomerClassificationAgency";
        private readonly ISentryClient _sentryClient;
        private readonly INewCustomerClassificationAgencyService _newCustomerClassificationAgencyService;
        private readonly IDataLogTrackingServices _dataLogTrackingServices;
        private readonly IAppSettings _appSettings;

        public NewCustomerClassificationAgencyJobService(
            ISentryClient sentryClient,
            INewCustomerClassificationAgencyService newCustomerClassificationAgencyService,
            IAppSettings appSettings,
            IDataLogTrackingServices dataLogTrackingServices)
        {
            _sentryClient = sentryClient;
            _newCustomerClassificationAgencyService = newCustomerClassificationAgencyService;
            _appSettings = appSettings;
            _dataLogTrackingServices = dataLogTrackingServices;
        }

        public async Task<bool> Execute(int batchSize)
        {
            try
            {
                return JobRun(batchSize);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex, _sentryClient);

                _dataLogTrackingServices.LogErrorData(ex, _serviceName);

                return false;
            }
        }

        #region Private Methods
        private bool JobRun(int batchSize)
        {
            var paramaters = new RescanParametersModel()
            {
                IsToQueue = false,
                FunctionId = FunctionId.InsertNewCustJob,
                RescanNormalType = RescanNormalType.Auto_Customer,
                InternalBatchSize = batchSize,
                ServiceName = _serviceName,
                PushDataExternal = new PushDataToExternalModel
                {
                    RoleGroup = RoleGroupType.Agency,
                    IsUpdateDangerToMainDB = false, // not update for sma
                    ExternalBatchSize = batchSize,
                    ServiceName = _serviceName,
                    IsSyncAFCData = _appSettings.IsSyncAFCData,
                    AFCInsertAccountURL = _appSettings.AFCInsertAccountURL,
                    AFCDeleteAccountURL = _appSettings.AFCDeleteAccountURL
                }
            };

            var isSuccess = _newCustomerClassificationAgencyService.Start()
                                .GetCategoryForNewCTSCustomerAgency()
                                .InsertVVIPForNewCustomerAgency(paramaters)
                                .InsertPAForNewCustomerAgency(paramaters)
                                .RescanNormalForNewCustomerAgency(paramaters)
                                .UpdateComplete();

            return isSuccess;
        }
        #endregion
    }
}
