namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer.Model;

    public interface ICTSCustomerAgencyService
    {
        InactivateNewCustomerModel GetInactivateNewCustomers();
    }

    public class CTSCustomerAgencyService : ICTSCustomerAgencyService
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
