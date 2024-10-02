namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System;
    using System.Linq;

    public interface IRealtimeClassificationAgencyService
    {
        IRealtimeClassificationAgencyService Start();

        IRealtimeClassificationAgencyService GetNewPlaceBetCustomers();

        IRealtimeClassificationAgencyService GetCustomerCategory(int batchSize);

        IRealtimeClassificationAgencyService PreprocessCustomerClassRealtime(int batchSize);

        IRealtimeClassificationAgencyService CompleteCustomerRealtime();
    }

    public class RealtimeClassificationAgencyService : IRealtimeClassificationAgencyService
    {
        private readonly IRealtimeClassificationAgencyDataAccess _dataAccess;
        private GetNewPlaceBetCustomerAgencyModel _customers;
        private GetCustomerCategoryAgencyRealtimeModel _customerCategories;

        public RealtimeClassificationAgencyService(IRealtimeClassificationAgencyDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IRealtimeClassificationAgencyService Start()
        {
            _customers = new GetNewPlaceBetCustomerAgencyModel();
            _customerCategories = new GetCustomerCategoryAgencyRealtimeModel();

            return this;
        }

        public IRealtimeClassificationAgencyService GetNewPlaceBetCustomers()
        {
            _customers = _dataAccess.GetNewPlaceBetCustomers();

            return this;
        }

        public IRealtimeClassificationAgencyService GetCustomerCategory(int batchSize)
        {
            if (_customers != null && _customers.CustIds.IsNotNullOrEmpty())
            {
                var customerChunks = _customers.CustIds.Chunk(batchSize);

                foreach (var custBatch in customerChunks)
                {
                    var custIDs = string.Join(",", custBatch.Distinct());

                    var custCategory = _dataAccess.GetCustomerCategory(custIDs);

                    _customerCategories.CustomerCategory.AddRange(custCategory.CustomerCategory);
                }
            }

            return this;
        }

        public IRealtimeClassificationAgencyService PreprocessCustomerClassRealtime(int batchSize)
        {
            if (_customerCategories != null && _customerCategories.CustomerCategory.IsNotNullOrEmpty())
            {
                var customerChunks = _customerCategories.CustomerCategory.Chunk(batchSize);

                foreach (var custBatch in customerChunks)
                {
                    var custIdList = string.Join(",", custBatch);

                    _dataAccess.PreprocessCustomerClassRealtime(custIdList);
                }
            }

            return this;
        }

        public IRealtimeClassificationAgencyService CompleteCustomerRealtime()
        {
            if (_customers != null && _customers.ScannedMaxId > 0)
            {
                _dataAccess.CompleteCustomerRealtime(_customers.ScannedMaxId);
            }

            return this;
        }
    }
}
