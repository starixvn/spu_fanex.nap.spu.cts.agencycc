namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Model;
    using System;
    using System.Linq;

    public interface IRealtimeClassificationAgencyDataAccess
    {
        GetNewPlaceBetCustomerAgencyModel GetNewPlaceBetCustomers();

        GetCustomerCategoryAgencyRealtimeModel GetCustomerCategory(string custIDList);

        void PreprocessCustomerClassRealtime(string custIDList);

        void InsertToQueuePaAgencyRealtime(string custIDList);

        void InsertToQueueConsiderableDanger(string custIDList);

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

        public GetCustomerCategoryAgencyRealtimeModel GetCustomerCategory(string custIDList)
        {
            var criteria = new GetCustomerCategoryAgencyRealtimeCriteria
            {
                CustIdList = custIDList
            };
            
            var (normalCusts, paCusts, considerableDangerCusts) = _repository.FetchMultiple<ulong, ulong, ulong>(criteria);

            return new GetCustomerCategoryAgencyRealtimeModel
            {
                CustomerCategory = normalCusts?.ToList(),
                CustomerPADailyScan = paCusts?.ToList(),
                CustomerConsiderableDanger = considerableDangerCusts?.ToList()
            };
        }

        public void PreprocessCustomerClassRealtime(string custIDList)
        {
            var criteria = new PreprocessCustomerAgencyRealtimeCriteria
            {
                ListCustId = custIDList
            };

            _repository.Execute(criteria);
        }

        public void InsertToQueuePaAgencyRealtime(string custIDList)
        {
            var criteria = new PaAgencyInsertToQueueRealtimeCriteria
            {
                CustIDList = custIDList
            };

            _repository.Execute(criteria);
        }

        public void InsertToQueueConsiderableDanger(string custIDList)
        {
            var criteria = new CdAgencyInsertToQueueCriteria
            {
                CustIDList = custIDList
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
