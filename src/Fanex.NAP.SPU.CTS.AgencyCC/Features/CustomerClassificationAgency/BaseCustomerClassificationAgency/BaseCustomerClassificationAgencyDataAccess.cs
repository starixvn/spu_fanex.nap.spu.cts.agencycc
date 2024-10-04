namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IBaseCustomerClassificationAgencyDataAccess
    {
        ResultInsertCustomerClassificationAgencyModel InsertCustomerClassification(AgencyCCInputFlow inputFlow, InsertCustomerClassificationAgencyModel model);

        GetRescanCustomerClassificationAgencyModel GetRescanCustomerClassificationAgency(string custIDs);
    }

    public class BaseCustomerClassificationAgencyDataAccess : IBaseCustomerClassificationAgencyDataAccess
    {
        private readonly IDynamicRepository _repository;

        public BaseCustomerClassificationAgencyDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public ResultInsertCustomerClassificationAgencyModel InsertCustomerClassification(AgencyCCInputFlow inputFlow, InsertCustomerClassificationAgencyModel model)
        {
            var criteria = new InsertCustomerClassificationAgencyCriteria
            {
                InputFlowID = (int)inputFlow,
                FromAction = model.FromAction,
                UplineRoleID = model.UplineRoleID,
                QueueID = model.QueueID,
                IsAffectDownline = model.IsAffectDownline,
                IsToQueue = model.IsToQueue,
                CustInfoJson = model.CustInfoJson
            };

            var custInfos = _repository.Fetch<CustInfoClassificationAgencyEntity>(criteria);

            return new ResultInsertCustomerClassificationAgencyModel { CustInfos = custInfos, ErrorMessage = criteria.ErrorMessage };
        }

        public GetRescanCustomerClassificationAgencyModel GetRescanCustomerClassificationAgency(string custIDs)
        {
            var criteria = new GetRescanCustomerClassificationAgencyCriteria
            {
                CustIDs = custIDs
            };

            var custInfos = _repository.Fetch<RescanCustomerClassificationAgencyEntity>(criteria);

            return new GetRescanCustomerClassificationAgencyModel { CustInfos = custInfos };
        }
    }
}
