namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Model;
    using System.Linq;

    public interface IRealtimeClassificationAgencyDataAccess
    {
        GetNewPlaceBetCustomerAgencyModel GetNewPlaceBetCustomers();

        GetCustomerCategoryAgencyRealtimeModel GetCustomerCategory(string custIdList);

        void PreprocessCustomerClassRealtime(string custIdList);

        void InsertToQueuePaAgencyRealtime(string custIdList);

        void CompleteCustomerRealtime(long scannedMaxId);
    }

    public class RealtimeClassificationAgencyDataAccess : IRealtimeClassificationAgencyDataAccess
    {
        private readonly IDynamicRepository _repository;

        public RealtimeClassificationAgencyDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public GetNewPlaceBetCustomerAgencyModel GetNewPlaceBetCustomers()
        {
            var criteria = new GetChangeCustomerAgencyRealtimeCriteria();

            var data = _repository.Fetch<GetNewPlaceBetCustomerAgencyEntity>(criteria);

            return new GetNewPlaceBetCustomerAgencyModel
            {
                CustomerNewPlaceBets = data,
                ScannedMaxId = criteria.ScannedMaxId
            };
        }

        public GetCustomerCategoryAgencyRealtimeModel GetCustomerCategory(string custIdList)
        {
            var criteria = new GetCustomerCategoryAgencyRealtimeCriteria
            {
                CustIdList = custIdList
            };

            var (normalCusts, paCusts) = _repository.FetchMultiple<long, long>(criteria);

            return new GetCustomerCategoryAgencyRealtimeModel
            {
                CustomerCategory = normalCusts?.ToList(),
                CustomerPADailyScan = paCusts?.ToList()
            };
        }

        public void PreprocessCustomerClassRealtime(string custIdList)
        {
            var criteria = new PreprocessCustomerAgencyRealtimeCriteria
            {
                ListCustId = custIdList
            };

            _repository.Execute(criteria);
        }

        public void InsertToQueuePaAgencyRealtime(string custIdList)
        {
            var criteria = new PaAgencyInsertToQueueRealtimeCriteria
            {
                CustIDList = custIdList
            };

            _repository.Execute(criteria);
        }

        public void CompleteCustomerRealtime(long scannedMaxId)
        {
            var criteria = new CompleteCustomerAgencyRealtimeCriteria
            {
                ScannedMaxId = scannedMaxId
            };

            _repository.Execute(criteria);
        }
    }
}
