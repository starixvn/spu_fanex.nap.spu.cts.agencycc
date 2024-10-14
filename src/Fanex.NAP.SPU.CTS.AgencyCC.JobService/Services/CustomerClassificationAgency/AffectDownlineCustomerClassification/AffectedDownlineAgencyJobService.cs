namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
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
        private readonly IDataLogTrackingServices _dataLogTrackingServices;

        public AffectedDownlineAgencyJobService(ISentryClient sentryClient,
            IAppSettings appSettings,
            IProblemAccountAgencyService problemAccountAgencyService,
            IAffectedDownlineAgencyService affectedDownlineAgency,
            IDataLogTrackingServices dataLogTrackingServices)
        {
            this._sentryClient = sentryClient;
            this._appSettings = appSettings;            
            this._dataLogTrackingServices = dataLogTrackingServices;
            this._problemAccountAgencyService = problemAccountAgencyService;
            this._affectedDownlineAgency = affectedDownlineAgency;
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
                        //isSuccess = _problemAccountAgencyService.RemoveVVIP();

                        break;
                    }
                case (int)DownlineClassification_ActionType.InsertPA:
                    {
                        isSuccess = InsertPA(agencyInfos);

                        break;
                    }
                case (int)DownlineClassification_ActionType.RemovePA:
                    {
                        //isSuccess = _problemAccountAgencyService.RemoveProblem();

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

                var result = _problemAccountAgencyService.InsertProblemAccountsAgencyPackage(probAccounts, FunctionId.DownlineCustomerClassification, AgencyCCInputFlow.InsertPACategory, agencyInfos.IsToQueue, serviceName);

                //return custClassToMainDBService.PushExternalProblemAcountPackage(result, agencyInfos.PushDataExternal);
            }

            return true;
        }

        public bool InsertVVIP(CustClassificationFromQueueModel agencyInfos)
        {
            var isSuccess = true;

            _affectedDownlineAgency.InsertVVIP(agencyInfos, FunctionId.DownlineCustomerClassification, AgencyCCInputFlow.InsertVVIP);

            return isSuccess;
        }

    }
}
