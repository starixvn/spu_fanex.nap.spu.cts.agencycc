namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer
{
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;

    public interface ICtsCustomerAgencyDataAccess
    {
        InactivateNewCustomerModel GetInactivateNewCustomers();
    }

    public class CtsCustomerAgencyDataAccess : ICtsCustomerAgencyDataAccess
    {
        private readonly IDynamicRepository _repository;

        public CtsCustomerAgencyDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public InactivateNewCustomerModel GetInactivateNewCustomers()
        {
            var data = new InactivateNewCustomerModel();
            var criteria = new GetInactivateNewCustomerCriteria();

            data.CustInfos = _repository.Fetch<RescanCategoryDailyModel>(criteria) ?? Enumerable.Empty<RescanCategoryDailyModel>();
            data.LastCustID = criteria.LastCustID;
            data.LastCategoryID = criteria.LastCategoryID;

            return data;
        }
    }
}
