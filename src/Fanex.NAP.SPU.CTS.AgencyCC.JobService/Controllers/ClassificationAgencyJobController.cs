namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.Shares;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/classificationAgencyJobs")]
    [ApiController]
    public class ClassificationAgencyJobController : ControllerBase
    {
        private readonly IRealtimeClassificationAgencyJobService _realtimeClassification;
        private readonly IDailyNormalClassificationAgencyJobService _dailyNormalClassification;
        private readonly INormalAccountClassificationAgencyJobService _normalAccountClassification;
        private readonly INormalAccountInsertAgencyJobService _normalAccountInsert;
        private readonly IDailyProblemClassificationAgencyJobService _problemClassificationAgencyJobService;

        public ClassificationAgencyJobController(
            IRealtimeClassificationAgencyJobService realtimeClassification,
            IDailyNormalClassificationAgencyJobService dailyNormalClassification,
            INormalAccountClassificationAgencyJobService normalAccountClassification,
            INormalAccountInsertAgencyJobService normalAccountInsert,
            IDailyProblemClassificationAgencyJobService problemClassificationAgencyJobService)
        {
            _realtimeClassification = realtimeClassification;
            _dailyNormalClassification = dailyNormalClassification;
            _normalAccountClassification = normalAccountClassification;
            _normalAccountInsert = normalAccountInsert;
            _problemClassificationAgencyJobService = problemClassificationAgencyJobService;
        }

        [HttpPost("scanningRealtimeClassificationAgency")]
        public async Task<bool> RealtimeScanningClassificationAgency([FromForm] int batchExternalSize, [FromForm] int batchInternalSize)
        {
            return await _realtimeClassification.Execute(batchExternalSize, batchInternalSize);
        }

        [HttpPost("scanningDailyNormalClassificationAgency")]
        public async Task<bool> DailyScanningNormalClassificationAgency()
        {
            LogExtension.LogInfo($"Daily Normal Agency Service is running at {DateTime.Now}");
            return await _dailyNormalClassification.Execute();
        }

        [HttpPost("normalAccountClassificationAgency")]
        public async Task<bool> NormalAccountClassification([FromForm] int batchInternalSize)
        {
            return await _normalAccountClassification.Execute(batchInternalSize);
        }

        [HttpPost("normalAccountInsertAgency")]
        public async Task<bool> NormalAccountInsert([FromForm] int batchExternalSize, [FromForm] int batchInternalSize)
        {
            return await _normalAccountInsert.Execute(batchExternalSize, batchInternalSize);
        }

        [HttpPost("scanningDailyProblemClassificationAgency")]
        public async Task<bool> DailyScanningProblemClassificationAgency([FromForm] int batchSize)
        {
            LogExtension.LogInfo($"Daily Problem Agency Service is running at {DateTime.Now}");
            return await _problemClassificationAgencyJobService.Execute(batchSize);
        }
    }
}
