namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.ArchiveData.ClassificationHistory
{
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Linq;

    public interface IArchiveHistoryClassificationDataAccess
    {
        IEnumerable<HistoryClassifyArchiveEntity> GetHistoryClassifyArchive();

        void InsertHistoryClassifyArchive(string historyDataJson);

        void CompleteHistoryClassifyArchive(IEnumerable<long> archiveIDs);
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

        public void CompleteHistoryClassifyArchive(IEnumerable<long> archiveIDs)
        {
            var criteria = new HistoryClassificationArchiveCompleteCriteria
            {
                MaxArchiveID = archiveIDs.Max(x => x),
                ListArchiveID = string.Join(",", archiveIDs)
            };

            _repository.Execute(criteria);
        }
    }
}
