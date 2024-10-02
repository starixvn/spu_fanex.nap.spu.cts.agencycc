namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;

    public interface IDailyNormalClassificationAgencyDataAccess
    {
        DailyNormalClassificationAgencyModel GetCustomerForDailyNormalScanning();

        void PreprocessNormalCustomer(string custIds);
    }

    public class DailyNormalClassificationAgencyDataAccess : IDailyNormalClassificationAgencyDataAccess
    {
        private readonly IDynamicRepository _repository;

        public DailyNormalClassificationAgencyDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public DailyNormalClassificationAgencyModel GetCustomerForDailyNormalScanning()
        {
            var criteria = new DailyNormalScanningAgencyGetCriteria();

            var result = _repository.Fetch<long>(criteria);

            return new DailyNormalClassificationAgencyModel
            {
                CustID = result,
                LastCategoryID = criteria.LastCategoryID,
                LastCustID = criteria.LastCustID
            };
        }

        public void PreprocessNormalCustomer(string custIds)
        {
            if (custIds.IsNotNullOrEmpty())
            {
                var criteria = new DailyNormalClassificationAgencyPreprocessCriteria()
                {
                    ListCustId = custIds
                };

                _repository.Execute(criteria);
            }
        }
    }
}
