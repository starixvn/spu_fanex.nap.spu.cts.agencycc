namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.ArchiveData.ClassificationHistory
{
    using System.Collections.Generic;

    public interface IArchiveHistoryClassificationService
    {
        IEnumerable<HistoryClassifyArchiveEntity> GetHistoryClassifyArchive();

        void InsertHistoryClassifyArchive(string historyDataJson);

        void CompleteHistoryClassifyArchive(IEnumerable<long> archiveIDs);
    }

    public class ArchiveHistoryClassificationService : IArchiveHistoryClassificationService
    {
        private readonly IArchiveHistoryClassificationDataAccess _dataAccess;

        public ArchiveHistoryClassificationService(IArchiveHistoryClassificationDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IEnumerable<HistoryClassifyArchiveEntity> GetHistoryClassifyArchive()
        {
            return _dataAccess.GetHistoryClassifyArchive();
        }

        public void InsertHistoryClassifyArchive(string historyDataJson)
        {
            _dataAccess.InsertHistoryClassifyArchive(historyDataJson);
        }

        public void CompleteHistoryClassifyArchive(IEnumerable<long> archiveIDs)
        {
            _dataAccess.CompleteHistoryClassifyArchive(archiveIDs);
        }

    }
}
