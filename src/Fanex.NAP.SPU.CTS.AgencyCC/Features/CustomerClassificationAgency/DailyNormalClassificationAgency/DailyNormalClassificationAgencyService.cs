namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IDailyNormalClassificationAgencyService
    {
        IDailyNormalClassificationAgencyService Start();

        IDailyNormalClassificationAgencyService GetCustomerForDailyNormalScanning();

        IDailyNormalClassificationAgencyService PreprocessDailyNormalCustomer();

        bool UpdateLastScan();
    }

    public class DailyNormalClassificationAgencyService : IDailyNormalClassificationAgencyService
    {
        private readonly IDailyNormalClassificationAgencyDataAccess _dataAccess;
        private readonly ISystemParameterServices _systemParameterService;
        private DailyNormalClassificationAgencyModel _scanningCustomers;

        public DailyNormalClassificationAgencyService(IDailyNormalClassificationAgencyDataAccess dataAccess, ISystemParameterServices systemParameterService)
        {
            _dataAccess = dataAccess;
            _systemParameterService = systemParameterService;
        }

        public IDailyNormalClassificationAgencyService Start()
        {
            _scanningCustomers = new DailyNormalClassificationAgencyModel();

            return this;
        }

        public IDailyNormalClassificationAgencyService GetCustomerForDailyNormalScanning()
        {
            _scanningCustomers = _dataAccess.GetCustomerForDailyNormalScanning();

            return this;
        }

        public IDailyNormalClassificationAgencyService PreprocessDailyNormalCustomer()
        {
            if (_scanningCustomers != null && _scanningCustomers.CustID.IsNotNullOrEmpty())
            {
                var custIds = string.Join(",", _scanningCustomers.CustID.Distinct());

                _dataAccess.PreprocessNormalCustomer(custIds);
            }

            return this;
        }

        public bool UpdateLastScan()
        {
            if (_scanningCustomers != null)
            {
                _systemParameterService.UpdateTimingExecution(_scanningCustomers.LastCustID.ToString(), CTS_SystemParameters.DailyScan_Agency_Normal_LastCustID);
                _systemParameterService.UpdateTimingExecution(_scanningCustomers.LastCategoryID.ToString(), CTS_SystemParameters.DailyScan_Agency_Normal_LastCategoryID);

                return (_scanningCustomers.CustID != null && _scanningCustomers.CustID.Any());
            }

            return false;
        }
    }
}
