namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares.Models;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IBaseCustomerClassificationAgencyDataAccess
    {
        ResultInsertCustomerClassificationAgencyModel InsertCustomerClassification(AgencyCCInputFlow inputFlow, InsertCustomerClassificationAgencyModel model);

        GetRescanCustomerClassificationAgencyModel GetRescanCustomerClassificationAgency(string custIDs);

        ResultRemoveCustomerClassificationAgencyModel RemoveCustomerClassificationAgency(AgencyCCInputFlow inputFlow, RemoveCustomerClassificationAgencyModel model);

        DBResult UpdateBetLimitOfFlaggedCustomer(string custIDs);

        DBResult UpdateBetLimitOfUnflaggedCustomer(string custIDs);
    }

    public class BaseCustomerClassificationAgencyDataAccess : IBaseCustomerClassificationAgencyDataAccess
    {
        protected readonly IDynamicRepository _repository;

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

        public ResultRemoveCustomerClassificationAgencyModel RemoveCustomerClassificationAgency(AgencyCCInputFlow inputFlow, RemoveCustomerClassificationAgencyModel model)
        {
            var criteria = new RemoveCustomerClassificationAgencyCriteria
            {
                InputFlowID = (int)inputFlow,
                FromAction = model.FromAction,
                UplineRoleID = model.UplineRoleID,
                QueueID = model.QueueID,
                IsAffectDownline = model.IsAffectDownline,
                IsFromQueue = model.IsFromQueue,
                CustClassificationJson = model.CustClassificationJson
            };

            var custInfos = _repository.Fetch<ResultRemoveCustomerClassificationEntity>(criteria);

            return new ResultRemoveCustomerClassificationAgencyModel { CustInfos = custInfos, ErrorMessage = criteria.ErrorMessage };
        }

        public DBResult UpdateBetLimitOfFlaggedCustomer(string custIDs)
        {
            var criteria = new BetLimitByEvidenceUpdateCriteria
            {
                CustIDList = custIDs
            };

            _repository.Execute(criteria);

            return new DBResult
            {
                ErrorCode = string.IsNullOrEmpty(criteria.ErrorMessage) ? 0 : -1,
                ErrorMsg = criteria.ErrorMessage
            };
        }

        public DBResult UpdateBetLimitOfUnflaggedCustomer(string custIDs)
        {
            var criteria = new BetLimitByEvidenceRemoveCriteria
            {
                CustIDList = custIDs
            };

            _repository.Execute(criteria);

            return new DBResult
            {
                ErrorCode = string.IsNullOrEmpty(criteria.ErrorMessage) ? 0 : -1,
                ErrorMsg = criteria.ErrorMessage
            };
        }

    }
}
