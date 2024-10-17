namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer
{
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;

#pragma warning disable S101 // Types should be named in PascalCase
    public interface ICTSCustomerAgencyDataAccess
#pragma warning restore S101 // Types should be named in PascalCase
    {
        InactivateNewCustomerModel GetInactivateNewCustomers();
    }

#pragma warning disable S101 // Types should be named in PascalCase
    public class CTSCustomerAgencyDataAccess : ICTSCustomerAgencyDataAccess
#pragma warning restore S101 // Types should be named in PascalCase
    {
        private readonly IDynamicRepository _repository;

        public CTSCustomerAgencyDataAccess()
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
