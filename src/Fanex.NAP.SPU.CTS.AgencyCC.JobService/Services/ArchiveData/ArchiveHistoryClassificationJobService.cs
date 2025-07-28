namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.ArchiveData
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.ArchiveData.ClassificationHistory;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IArchiveHistoryClassificationJobService
    {
        Task<bool> Execute();
    }

    public class ArchiveHistoryClassificationJobService : IArchiveHistoryClassificationJobService
    {
        private readonly IArchiveHistoryClassificationService _service;

        public ArchiveHistoryClassificationJobService(IArchiveHistoryClassificationService jobService)
        {
            this._service = jobService;
        }

        public Task<bool> Execute()
        {
            try
            {
                var data = _service.GetHistoryClassifyArchive();

                if (!data.IsNullOrEmpty())
                {
                    _service.InsertHistoryClassifyArchive(JsonConvert.SerializeObject(data));

                    _service.CompleteHistoryClassifyArchive(data.Select(x=>x.ID));
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(nameof(ArchiveHistoryClassificationJobService), ex.Message, string.Empty, ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
