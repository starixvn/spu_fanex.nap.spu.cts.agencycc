namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer.Model;

#pragma warning disable S101 // Types should be named in PascalCase
    public interface ICTSCustomerAgencyService
#pragma warning restore S101 // Types should be named in PascalCase
    {
        InactivateNewCustomerModel GetInactivateNewCustomers();
    }

#pragma warning disable S101 // Types should be named in PascalCase
    public class CTSCustomerAgencyService : ICTSCustomerAgencyService
#pragma warning restore S101 // Types should be named in PascalCase
    {
        private readonly ICTSCustomerAgencyDataAccess _dataAccess;

        public CTSCustomerAgencyService(ICTSCustomerAgencyDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public InactivateNewCustomerModel GetInactivateNewCustomers()
        {
            return _dataAccess.GetInactivateNewCustomers();
        }
    }
}
