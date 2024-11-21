namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Model;
    using System.Collections.Generic;

    public interface INormalAccountClassificationAgencyDataAccess
    {
        GetNormalPoolCustomerAgencyModel GetNormalPoolCustomers();

        IEnumerable<GetCurrentNormalCategoryAgencyEntity> GetCurrentCustomerCategory(string custIDList);

        void ClassifyNormalPoolCustomers(string customerXML);

        void ClearNormalPoolCustomers(string custIdList, long scannedMaxId);
    }

    public class NormalAccountClassificationAgencyDataAccess : INormalAccountClassificationAgencyDataAccess
    {
        private readonly IDynamicRepository _repository;

        public NormalAccountClassificationAgencyDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public GetNormalPoolCustomerAgencyModel GetNormalPoolCustomers()
        {
            var criteria = new GetNormalPoolCustomerAgencyCriteria();

            var data = _repository.Fetch<GetNormalPoolCustomerAgencyEntity>(criteria);

            return new GetNormalPoolCustomerAgencyModel
            {
                NormalPoolCustomers = data,
                ScannedMaxId = criteria.ScannedMaxId
            };
        }

        public IEnumerable<GetCurrentNormalCategoryAgencyEntity> GetCurrentCustomerCategory(string custIDList)
        {
            var criteria = new GetCurrentNormalCategoryAgencyCriteria
            {
                CustIDs = custIDList
            };

            return _repository.Fetch<GetCurrentNormalCategoryAgencyEntity>(criteria);
        }

        public void ClassifyNormalPoolCustomers(string customerXML)
        {
            var criteria = new ClassifyNormalPoolCustomerAgencyCriteria
            {
                CustomersXML = customerXML
            };

            _repository.Execute(criteria);
        }

        public void ClearNormalPoolCustomers(string custIdList, long scannedMaxId)
        {
            var criteria = new ClearNormalPoolCustomerAgencyCriteria
            {
                ListCustId = custIdList,
                ScannedMaxId = scannedMaxId
            };

            _repository.Execute(criteria);
        }
    }
}
