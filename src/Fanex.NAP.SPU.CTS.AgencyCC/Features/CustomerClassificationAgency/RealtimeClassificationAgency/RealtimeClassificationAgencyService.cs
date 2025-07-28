namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IRealtimeClassificationAgencyService
    {
        IRealtimeClassificationAgencyService Start();

        IRealtimeClassificationAgencyService GetNewPlaceBetCustomers();

        IRealtimeClassificationAgencyService GetCustomerCategory(int batchSize);

        IRealtimeClassificationAgencyService PreprocessCustomerClassRealtime(int batchSize);

        IRealtimeClassificationAgencyService InsertToQueuePaAgencyRealtime(int batchSize);

        IRealtimeClassificationAgencyService InsertToQueueConsiderableDanger(int batchSize, AgencyScanType agencyScanType);

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
            if (_customers != null && _customers.CustomerNewPlaceBets.IsNotNullOrEmpty())
            {
                var customerChunks = _customers.CustomerNewPlaceBets.Chunk(batchSize);

                foreach (var custBatch in customerChunks)
                {
                    var custIDs = string.Join(",", custBatch.Select(x => x.CustId).Distinct());

                    var custCategory = _dataAccess.GetCustomerCategory(custIDs);

                    _customerCategories.AddItems(custCategory);
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
                    _dataAccess.PreprocessCustomerClassRealtime(string.Join(",", custBatch));
                }
            }

            return this;
        }

        public IRealtimeClassificationAgencyService InsertToQueuePaAgencyRealtime(int batchSize)
        {
            if (_customerCategories != null && _customerCategories.CustomerPADailyScan.IsNotNullOrEmpty()) 
            {
                var paCustChunks = _customerCategories.CustomerPADailyScan.Chunk(batchSize);

                foreach (var custBatch in paCustChunks)
                {                    
                    _dataAccess.InsertToQueuePaAgencyRealtime(string.Join(",", custBatch));
                }
            }

            return this;
        }

        public IRealtimeClassificationAgencyService InsertToQueueConsiderableDanger(int batchSize, AgencyScanType agencyScanType)
        {
            if (_customerCategories != null && _customerCategories.CustomerConsiderableDanger.IsNotNullOrEmpty())
            {
                var cdCustChunks = _customerCategories.CustomerPADailyScan.Chunk(batchSize);

                foreach (var custBatch in cdCustChunks)
                {                   
                    _dataAccess.InsertToQueueConsiderableDanger(string.Join(",", custBatch), agencyScanType);
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
