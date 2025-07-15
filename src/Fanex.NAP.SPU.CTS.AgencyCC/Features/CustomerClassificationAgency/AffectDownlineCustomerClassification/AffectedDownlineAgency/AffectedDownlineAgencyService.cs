namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
    using Newtonsoft.Json;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IAffectedDownlineAgencyService
    {
        ResultInsertCustomerClassificationAgencyModel InsertVVIP(CustClassificationFromQueueModel agencyInfos, FunctionId funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue);

        ResultRemoveCustomerClassificationAgencyModel RemoveVVIP(CustClassificationFromQueueModel agencyInfos);
    }

    public class AffectedDownlineAgencyService : BaseCustomerClassificationAgencyService, IAffectedDownlineAgencyService
    {
        private readonly string serviceName;

        public AffectedDownlineAgencyService(IBaseCustomerClassificationAgencyDataAccess baseCCAgencyDataAccess) : base(baseCCAgencyDataAccess) {
            this.serviceName = "AffectedDownlineAgencyService";
        }

        public ResultInsertCustomerClassificationAgencyModel InsertVVIP(CustClassificationFromQueueModel agencyInfos, FunctionId funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue)
        {
            var custInfo = MappingVVIPModel(agencyInfos);

            var modelInsert = new InsertCustomerClassificationAgencyModel
            {
                CustInfoJson = JsonConvert.SerializeObject(custInfo.CustInfo),
                FromAction = (int)custInfo.FromFlow,
                UplineRoleID = custInfo.UplineRoleID,
                QueueID = custInfo.QueueID,
                IsAffectDownline = custInfo.IsAffectDownline,
                IsToQueue = isToQueue
            };

            return base.InsertCustomerClassification(ccInputFlow, modelInsert, serviceName);          
        }

        public ResultRemoveCustomerClassificationAgencyModel RemoveVVIP(CustClassificationFromQueueModel agencyInfos)
        { 
            var custInfo = MappingVVIPModel(agencyInfos);

            if (custInfo != null && custInfo.CustInfo.Any())
            {
                var modelRemove = new RemoveCustomerClassificationAgencyModel
                {
                    CustClassificationJson = JsonConvert.SerializeObject(custInfo.CustInfo.Select(c => new RemoveCustomerClassificationEntity
                    {
                        CustID = c.CustID,
                        CTSCustID = c.CTSCustID,
                        CreatedBy = c.CreatedBy
                    })),
                    FromAction = (int)custInfo.FromFlow,
                    UplineRoleID = agencyInfos.UplineRoleID,
                    QueueID = custInfo.QueueID,
                    IsAffectDownline = custInfo.IsAffectDownline,
                    IsFromQueue = false
                };

                return base.RemoveCustomerClassificationAgency(AgencyCCInputFlow.RemoveVVIP, modelRemove);                
            }

            return new ResultRemoveCustomerClassificationAgencyModel();
        }

        private VvipCustomerInsertEntity MappingVVIPModel(CustClassificationFromQueueModel agencyInfo)
        {
            return new VvipCustomerInsertEntity
            {
                FromFlow = FromFlow.CTSServices,
                IsAffectDownline = false,
                QueueID = agencyInfo.QueueID,
                UplineRoleID = agencyInfo.UplineRoleID,
                CustInfo = agencyInfo.AgencyQueue.Select(c => new VvipCategoryEntity
                {
                    CustID = c.CustID,
                    CTSCustID = c.CTSCustID,
                    SubscriberID = c.SubscriberID,
                    CategoryID = c.CategoryID,
                    RoleID = c.RoleID,
                    Remark = c.Remark,
                    CreatedBy = c.CreatedBy
                })
            };
        }

    }
}
