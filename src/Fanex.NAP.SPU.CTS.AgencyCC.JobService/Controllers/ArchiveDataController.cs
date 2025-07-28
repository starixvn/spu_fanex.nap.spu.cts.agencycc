namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.ArchiveData;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/ArchiveData")]
    public class ArchiveDataController : ControllerBase
    {
        private readonly IArchiveHistoryClassificationJobService _arcHisClassJobService;

        public ArchiveDataController(IArchiveHistoryClassificationJobService arcHisClassJobService)
        {
            _arcHisClassJobService = arcHisClassJobService;
        }

        [HttpPost("ArchiveHistoryClassification")]
        public async Task<bool> ArchiveHistoryClassification()
        {
            return await this._arcHisClassJobService.Execute();
        }
    }
}
