namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Entities;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IRescanCustClassificationAgencyDataAccess
    {
        IEnumerable<RoleCustomerEntity> GetRoleCustomer(string custIds);

        int PreNormalClassification(string custList, FunctionId functionId, RescanNormalType type);
    }

    public class RescanCustClassificationAgencyDataAccess : IRescanCustClassificationAgencyDataAccess
    {
        private readonly IDynamicRepository _repository;

        public RescanCustClassificationAgencyDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public IEnumerable<RoleCustomerEntity> GetRoleCustomer(string custIds)
        {
            var criteria = new RoleCustomerGetCriteria
            {
                CustIds = custIds
            };

            return _repository.Fetch<RoleCustomerEntity>(criteria);
        }

        public int PreNormalClassification(string custList, FunctionId functionId, RescanNormalType type)
        {
            var criteria = new NormalClassificationAgencyRescanCriteria
            {
                ListCustId = custList,
                FunctionId = (int)functionId,
                RescanTypeId = (int)type

            };

            return _repository.Execute(criteria);
        }
    }
}
