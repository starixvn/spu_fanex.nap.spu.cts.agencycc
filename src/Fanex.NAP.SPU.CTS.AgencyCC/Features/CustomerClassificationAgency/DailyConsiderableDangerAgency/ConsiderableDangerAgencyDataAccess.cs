namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{
    using Fanex.Data.Repository;    
    using System.Collections.Generic;
    using System.Linq;   

    public interface IConsiderableDangerAgencyDataAccess
    {
        void UpdateConsiderableDangerQueueMaxID();

        ConsiderableDangerRecheckModel GetConsiderableDangerQueue(int batchSize);

        IEnumerable<DetectConsiderableEntity> DetectConsiderable(IEnumerable<long> custIDs);

        void CompleteConsiderableQueue(long lastID);

    }

    public class ConsiderableDangerAgencyDataAccess : IConsiderableDangerAgencyDataAccess
    {
        private readonly IDynamicRepository _repository;

        public ConsiderableDangerAgencyDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public void UpdateConsiderableDangerQueueMaxID()
        {
            var criteria = new ConsiderableDangerQueueSetMaxIDCriteria();

            _repository.Execute(criteria);
        }        

        public ConsiderableDangerRecheckModel GetConsiderableDangerQueue(int batchSize)
        {
            var criteria = new ConsiderableDangerQueueGetCriteria { BatchSize = batchSize };

            var (recheckRatioCusts, recheckWLCusts) = _repository.FetchMultiple<ConsiderableDangerRecheckEntity, ConsiderableDangerRecheckEntity>(criteria);

            return new ConsiderableDangerRecheckModel
            {
                LastID = criteria.LastID,
                RecheckRatioCusts = recheckRatioCusts?.ToList(),
                RecheckWLCusts = recheckWLCusts?.ToList()
            };
        }

        public IEnumerable<DetectConsiderableEntity> DetectConsiderable(IEnumerable<long> custIDs)
        {
            var criteria = new ConsiderableDangerDetectCriteria
            {
                CustIDs = string.Join(",", custIDs),
                IsRescan = false
            };

            return _repository.Fetch<DetectConsiderableEntity>(criteria);
        }

        public void CompleteConsiderableQueue(long lastID)
        {
            var criteria = new ConsiderableDangerQueueCompleteCriteria { LastID = lastID };
            
            _repository.Execute(criteria);
        }
    }
}
