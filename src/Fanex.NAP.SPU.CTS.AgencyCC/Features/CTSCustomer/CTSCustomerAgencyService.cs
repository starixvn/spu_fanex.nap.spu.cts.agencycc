namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer.Model;

    public interface ICtsCustomerAgencyService
    {
        InactivateNewCustomerModel GetInactivateNewCustomers();
    }

    public class CtsCustomerAgencyService : ICtsCustomerAgencyService
    {
        private readonly ICtsCustomerAgencyDataAccess _dataAccess;

        public CtsCustomerAgencyService(ICtsCustomerAgencyDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public InactivateNewCustomerModel GetInactivateNewCustomers()
        {
            return _dataAccess.GetInactivateNewCustomers();
        }
    }
}
