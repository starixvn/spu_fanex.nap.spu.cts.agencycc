namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.ArchiveData.ClassificationHistory
{
    using Fanex.Data.Repository;
    using System.Collections.Generic;    

    public interface IArchiveHistoryClassificationDataAccess
    {
        IEnumerable<HistoryClassifyArchiveEntity> GetHistoryClassifyArchive();

        void InsertHistoryClassifyArchive(string historyDataJson);

        void CompleteHistoryClassifyArchive(IEnumerable<long> IDs);
    }

    public class ArchiveHistoryClassificationDataAccess : IArchiveHistoryClassificationDataAccess
    {
        private readonly IDynamicRepository _repository;

        public ArchiveHistoryClassificationDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public IEnumerable<HistoryClassifyArchiveEntity> GetHistoryClassifyArchive()
        {           
            var criteria = new HistoryClassificationArchiveGetCriteria();

            return _repository.Fetch<HistoryClassifyArchiveEntity>(criteria);
        }

        public void InsertHistoryClassifyArchive(string historyDataJson)
        {
            var criteria = new HistoryClassificationArchiveInsertCriteria
            {
                HistoryDataJson = historyDataJson
            };

            _repository.Execute(criteria);
        }

        public void CompleteHistoryClassifyArchive(IEnumerable<long> IDs)
        {
            var criteria = new HistoryClassificationArchiveCompleteCriteria
            {
                IDs = string.Join(",", IDs)
            };

            _repository.Execute(criteria);
        }
    }
}
