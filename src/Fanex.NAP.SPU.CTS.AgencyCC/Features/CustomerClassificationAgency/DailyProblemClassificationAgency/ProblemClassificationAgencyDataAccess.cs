namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency.Entities;
    using System.Collections.Generic;

    public interface IProblemClassificationAgencyDataAccess
    {
        IEnumerable<long> GetPADailyScanFromQueue();

        void CompletePADailyScan(string custIDs);

        IEnumerable<PAWLInsertAgencyEntity> InsertProblemAccountWLPerformance(string paJson);
    }

    public class ProblemClassificationAgencyDataAccess : IProblemClassificationAgencyDataAccess
    {
        private readonly IDynamicRepository _repository;

        public ProblemClassificationAgencyDataAccess(IDynamicRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<long> GetPADailyScanFromQueue()
        {
            var criteria = new PADailyScanFromQueueAgencyCriteria();

            return _repository.Fetch<long>(criteria);
        }

        public void CompletePADailyScan(string custIDs)
        {
            var criteria = new PADailyScanCompleteAgencyCriteria()
            {
                CustIDs = custIDs
            };

            _repository.Execute(criteria);
        }

        public IEnumerable<PAWLInsertAgencyEntity> InsertProblemAccountWLPerformance(string paJson)
        {
            var criteria = new PAWLInsertAgencyCriteria()
            {
                ProblemAccountJson = paJson
            };

            return _repository.Fetch<PAWLInsertAgencyEntity>(criteria);
        }
    }
}
