namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.AntiFraudCenter;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgencyAPI.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.DataLogTracking;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Sentry;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IAffectedDownlineAgencyJobService
    {
        Task<bool> Execute(CustomerClassificationAgencyParameterModel<CustClassificationFromQueueModel> agencyInfos);
    }

    public class AffectedDownlineAgencyJobService : IAffectedDownlineAgencyJobService
    {
        private readonly string _serviceName;
        private readonly ISentryClient _sentryClient;
        private readonly IAppSettings _appSettings;
        private readonly IProblemAccountAgencyService _problemAccountAgencyService;
        private readonly IAffectedDownlineAgencyService _affectedDownlineAgency;
        private readonly ICustClassToMainDBService _custClassToMainDBService;
        private readonly IAntiFraudCenterServices _antiFraudCenterServices;
        private readonly IRescanCustClassificationAgencyService _rescanCustClassificationService;
        private readonly IDataLogTrackingServices _dataLogTrackingServices;

#pragma warning disable S107
        public AffectedDownlineAgencyJobService(
            ISentryClient sentryClient,
            IAppSettings appSettings,
            IProblemAccountAgencyService problemAccountAgencyService,
            IRescanCustClassificationAgencyService rescanCustClassificationService,
            IAffectedDownlineAgencyService affectedDownlineAgency,
            IDataLogTrackingServices dataLogTrackingServices,
            ICustClassToMainDBService custClassToMainDBService,
            IAntiFraudCenterServices antiFraudCenterServices)
#pragma warning restore S107
        {
            _sentryClient = sentryClient;
            _appSettings = appSettings;
            _dataLogTrackingServices = dataLogTrackingServices;
            _problemAccountAgencyService = problemAccountAgencyService;
            _affectedDownlineAgency = affectedDownlineAgency;
            _rescanCustClassificationService = rescanCustClassificationService;
            _serviceName = "AffectedDownlineCustClassificationAgency";
            _custClassToMainDBService = custClassToMainDBService;
            _antiFraudCenterServices = antiFraudCenterServices;
        }

        public Task<bool> Execute(CustomerClassificationAgencyParameterModel<CustClassificationFromQueueModel> agencyInfos)
        {
            try
            {
                return Task.FromResult(JobRun(agencyInfos));
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex, _sentryClient);

                _dataLogTrackingServices.LogErrorData(ex, _serviceName);

                return Task.FromResult(false);
            }
        }

        private bool JobRun(CustomerClassificationAgencyParameterModel<CustClassificationFromQueueModel> agencyInfos)
        {
            var isSuccess = true;
            var actionType = agencyInfos.Data.ActionType;

            var rescanParamaters = new RescanParametersModel
            {
                IsToQueue = false,
                FunctionId = FunctionId.DownlineCustomerClassification,
                RescanNormalType = RescanNormalType.Auto_AfftectedDownline,
                InternalBatchSize = agencyInfos.InternalBatchSize,
                ServiceName = _serviceName,
                PushDataExternal = new PushDataToExternalModel
                {
                    RoleGroup = RoleGroupType.Agency,
                    IsUpdateDangerToMainDB = false, // not update for sma
                    ExternalBatchSize = agencyInfos.ExternalBatchSize,
                    ServiceName = _serviceName,
                    IsSyncAFCData = _appSettings.IsSyncAFCData,
                    AFCInsertAccountURL = _appSettings.AFCInsertAccountURL,
                    AFCDeleteAccountURL = _appSettings.AFCDeleteAccountURL
                }
            };

#pragma warning disable S131
            switch (actionType)
            {
                case (int)DownlineClassification_ActionType.InsertVVIP:
                    {
                        isSuccess = InsertVVIP(agencyInfos.Data, rescanParamaters);

                        break;
                    }
                case (int)DownlineClassification_ActionType.RemoveVVIP:
                    {
                        isSuccess = RemoveVVIP(agencyInfos.Data, rescanParamaters);

                        break;
                    }
                case (int)DownlineClassification_ActionType.InsertPA:
                    {
                        isSuccess = InsertPA(agencyInfos.Data, rescanParamaters);

                        break;
                    }
                case (int)DownlineClassification_ActionType.RemovePA:
                    {
                        isSuccess = RemovePA(agencyInfos.Data, rescanParamaters);

                        break;
                    }
            }
#pragma warning restore S131

            return isSuccess;
        }

        private bool InsertPA(CustClassificationFromQueueModel agencyInfos, RescanParametersModel rescanParameters)
        {
            if (agencyInfos != null && agencyInfos.AgencyQueue.Any())
            {
                var probAccounts = agencyInfos.AgencyQueue.Select(c => new InsertProblemAccountEntity
                {
                    CustID = c.CustID,
                    CTSCustID = c.CTSCustID,
                    RoleID = c.RoleID,
                    SubscriberID = c.SubscriberID,
                    CategoryID = c.CategoryID,
                    CategoryGroupID = c.CategoryGroup,
                    IsLicensee = c.IsLicensee,
                    Remark = c.Remark,
                    IsMarkedDirectly = false,
                    IsFromCTS = c.IsFromCTS,
                    CreatedBy = c.CreatedBy
                });

                var result = _problemAccountAgencyService.InsertProblemAccountsAgencyPackage(probAccounts,
                    FunctionId.DownlineCustomerClassification,
                    AgencyCCInputFlow.InsertPACategory,
                    rescanParameters.IsToQueue, _serviceName);

                return _custClassToMainDBService.PushExternalProblemAcountPackage(result, rescanParameters.PushDataExternal);
            }

            return true;
        }

        private bool RemovePA(CustClassificationFromQueueModel agencyInfos, RescanParametersModel rescanParameters)
        {
            var isSuccess = true;

            if (agencyInfos != null && agencyInfos.AgencyQueue.Any())
            {
                var probAccounts = agencyInfos.AgencyQueue.Select(c => new RemoveProblemAccountEntity
                {
                    CustID = c.CustID,
                    CTSCustID = c.CTSCustID,
                    RoleID = c.RoleID,
                    SubscriberID = c.SubscriberID,
                    Remark = c.Remark,
                    IsMarkedDirectly = false,
                    IsExceptDirectDownline = c.IsExceptDirectDownline,
                    CreatedBy = c.CreatedBy
                });

                var result = _problemAccountAgencyService.RemoveProblemAccount(probAccounts, true);

                isSuccess = string.IsNullOrEmpty(result.ErrorMessage);

                if (isSuccess && result.CustInfos != null && result.CustInfos.Any())
                {
                    isSuccess = _rescanCustClassificationService.RescanCustomerClassificationAgency(result.CustInfos.Select(c => c.CustID).Distinct(), rescanParameters);
                }
            }

            return isSuccess;
        }

        private bool InsertVVIP(CustClassificationFromQueueModel agencyInfos, RescanParametersModel parameters)
        {
            var isSuccess = true;

            var result = _affectedDownlineAgency.InsertVVIP(agencyInfos, FunctionId.DownlineCustomerClassification, AgencyCCInputFlow.InsertVVIP, parameters.IsToQueue);

            isSuccess = string.IsNullOrEmpty(result.ErrorMessage);

            if (isSuccess && result.CustInfos != null && result.CustInfos.Any())
            {
                var custIDs = result.CustInfos.Select(c => c.CustID).Distinct();

                isSuccess = _custClassToMainDBService.PushExternalPackageByCustId(custIDs, parameters.PushDataExternal);

                _antiFraudCenterServices.DeleteAFCAccounts(custIDs, FromFlow.CTSServices, parameters.PushDataExternal.IsSyncAFCData, parameters.PushDataExternal.AFCDeleteAccountURL);
            }

            return isSuccess;
        }

        private bool RemoveVVIP(CustClassificationFromQueueModel agencyInfos, RescanParametersModel parameters)
        {
            var isSuccess = true;

            var result = _affectedDownlineAgency.RemoveVVIP(agencyInfos);

            isSuccess = string.IsNullOrEmpty(result.ErrorMessage);

            if (isSuccess && result.CustInfos != null && result.CustInfos.Any())
            {
                isSuccess = _rescanCustClassificationService.RescanCustomerClassificationAgency(result.CustInfos.Select(c => c.CustID).Distinct(), parameters);
            }

            return isSuccess;
        }
    }
}
