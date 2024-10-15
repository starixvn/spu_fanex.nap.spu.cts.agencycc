namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.DataLogTracking;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IAffectedDownlineAgencyJobService
    {
        Task<bool> Execute(CustClassificationFromQueueModel agencyInfos);
    }

    public class AffectedDownlineAgencyJobService : IAffectedDownlineAgencyJobService
    {
        private readonly string serviceName;
        private readonly ISentryClient _sentryClient;
        private readonly IAppSettings _appSettings;
        private readonly IProblemAccountAgencyService _problemAccountAgencyService;
        private readonly IAffectedDownlineAgencyService _affectedDownlineAgency;
        private readonly IRescanCustClassificationAgencyService _rescanCustClassificationService;
        private readonly IDataLogTrackingServices _dataLogTrackingServices;

        public AffectedDownlineAgencyJobService(ISentryClient sentryClient,
            IAppSettings appSettings,
            IProblemAccountAgencyService problemAccountAgencyService,
            IRescanCustClassificationAgencyService _rescanCustClassificationService,
            IAffectedDownlineAgencyService affectedDownlineAgency,
            IDataLogTrackingServices dataLogTrackingServices)
        {
            this._sentryClient = sentryClient;
            this._appSettings = appSettings;            
            this._dataLogTrackingServices = dataLogTrackingServices;
            this._problemAccountAgencyService = problemAccountAgencyService;
            this._affectedDownlineAgency = affectedDownlineAgency;
            this._rescanCustClassificationService = _rescanCustClassificationService;
            this.serviceName = "AffectedDownlineCustClassificationAgency";
        }

        public Task<bool> Execute(CustClassificationFromQueueModel agencyInfos)
        {
            try
            {
                return Task.FromResult(JobRun(agencyInfos));
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex, _sentryClient);

                _dataLogTrackingServices.LogErrorData(ex, serviceName);

                return Task.FromResult(false);
            }
        }

        private bool JobRun(CustClassificationFromQueueModel agencyInfos)
        {
            var isSuccess = true;
            var actionType = agencyInfos.ActionType;

            switch (actionType)
            {
                case (int)DownlineClassification_ActionType.InsertVVIP:
                    {
                        isSuccess = InsertVVIP(agencyInfos);

                        break;
                    }
                case (int)DownlineClassification_ActionType.RemoveVVIP:
                    {
                        isSuccess = RemoveVVIP(agencyInfos);

                        break;
                    }
                case (int)DownlineClassification_ActionType.InsertPA:
                    {
                        isSuccess = InsertPA(agencyInfos);

                        break;
                    }
                case (int)DownlineClassification_ActionType.RemovePA:
                    {
                        isSuccess = RemovePA(agencyInfos);

                        break;
                    }
            }

            return isSuccess;
        }

        private bool InsertPA(CustClassificationFromQueueModel agencyInfos)
        {
            if (agencyInfos != null && agencyInfos.AgencyQueue.Any())
            {
                var custIds = agencyInfos.AgencyQueue.Select(c => c.CustID).Distinct();

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
                    agencyInfos.RescanParameters.IsToQueue, serviceName);

                //return custClassToMainDBService.PushExternalProblemAcountPackage(result, agencyInfos.PushDataExternal);
            }

            return true;
        }

        private bool RemovePA(CustClassificationFromQueueModel agencyInfos)
        {
            var isSuccess = true;

            if (agencyInfos != null && agencyInfos.AgencyQueue.Any())
            {
                var custIds = agencyInfos.AgencyQueue.Select(c => c.CustID).Distinct();

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

                if (string.IsNullOrEmpty(result.ErrorMessage) && result.CustInfos != null && result.CustInfos.Any())
                {
                    isSuccess = _rescanCustClassificationService.RescanCustomerClassificationAgency(result.CustInfos.Select(c => c.CustID).Distinct(), agencyInfos.RescanParameters);
                }               
            }

            return true;
        }

        public bool InsertVVIP(CustClassificationFromQueueModel agencyInfos)
        {
            var isSuccess = true;

            var result = _affectedDownlineAgency.InsertVVIP(agencyInfos, FunctionId.DownlineCustomerClassification, AgencyCCInputFlow.InsertVVIP, agencyInfos.RescanParameters.IsToQueue);

            isSuccess = string.IsNullOrEmpty(result.ErrorMessage);

            if (isSuccess && result.CustInfos != null && result.CustInfos.Any())
            {
                var custIDs = result.CustInfos.Select(c => c.CustID).Distinct();

                //isSuccess = custClassToMainDBService.PushExternalPackageByCustId(custIDs, new PushDataToExternalEntity
                //{
                //    ExternalBatchSize = parameters.PushDataExternal.ExternalBatchSize,
                //    IsUpdateDangerToMainDB = false,
                //    IsSyncAFCData = false,
                //    ServiceName = parameters.ServiceName
                //});

                //antiFraudCenterServices.DeleteAFCAccounts(custIDs, FromFlow.CTSServices, parameters.PushDataExternal.IsSyncAFCData, parameters.PushDataExternal.AFCDeleteAccountURL);
            }
            
            return isSuccess;
        }

        public bool RemoveVVIP(CustClassificationFromQueueModel agencyInfos)
        {
            var isSuccess = true;

            var result = _affectedDownlineAgency.RemoveVVIP(agencyInfos);

            isSuccess = string.IsNullOrEmpty(result.ErrorMessage);

            if (isSuccess && result.CustInfos != null && result.CustInfos.Any())
            {
                isSuccess = _rescanCustClassificationService.RescanCustomerClassificationAgency(result.CustInfos.Select(c => c.CustID).Distinct(), agencyInfos.RescanParameters);
            }

            return isSuccess;
        }
    }
}
