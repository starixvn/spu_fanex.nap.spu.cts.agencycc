namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IAffectedDownlineAgencyService
    {
        IEnumerable<long> InsertVVIP(CustClassificationFromQueueModel agencyInfos, FunctionId funcId, AgencyCCInputFlow ccInputFlow);
    }

    public class AffectedDownlineAgencyService : BaseCustomerClassificationAgencyService, IAffectedDownlineAgencyService
    {
        private readonly string serviceName;

        public AffectedDownlineAgencyService(IBaseCustomerClassificationAgencyDataAccess baseCCAgencyDataAccess,
            ISentryClient sentryClient) : base(baseCCAgencyDataAccess, sentryClient) {
            this.serviceName = "AffectedDownlineAgencyService";
        }

        public IEnumerable<long> InsertVVIP(CustClassificationFromQueueModel agencyInfos, FunctionId funcId, AgencyCCInputFlow ccInputFlow)
        {
            var custInfo = MappingVVIPModel(agencyInfos);

            var modelInsert = new InsertCustomerClassificationAgencyModel()
            {
                CustInfoJson = JsonConvert.SerializeObject(custInfo.CustInfo),
                FromAction = (int)custInfo.FromFlow,
                UplineRoleID = custInfo.UplineRoleID,
                QueueID = custInfo.QueueID,
                IsAffectDownline = custInfo.IsAffectDownline,
                IsToQueue = agencyInfos.IsToQueue
            };

            var result = base.InsertCustomerClassification(ccInputFlow, modelInsert, serviceName);
          
            if (result.CustInfos != null && result.CustInfos.Any())
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

            return Enumerable.Empty<long>();
        }

        private VVIPCustomerInsertEntity MappingVVIPModel(CustClassificationFromQueueModel agencyInfo)
        {
            return new VVIPCustomerInsertEntity
            {
                FromFlow = FromFlow.CTSServices,
                IsAffectDownline = false,
                QueueID = agencyInfo.QueueID,
                UplineRoleID = agencyInfo.UplineRoleID,
                CustInfo = agencyInfo.AgencyQueue.Select(c => new VVIPCategoryEntity
                {
                    CustID = c.CustID,
                    CTSCustID = c.CTSCustID,
                    SubscriberID = c.SubscriberID,
                    RoleID = c.RoleID,
                    Remark = c.Remark,
                    CreatedBy = c.CreatedBy
                })
            };
        }

    }
}
