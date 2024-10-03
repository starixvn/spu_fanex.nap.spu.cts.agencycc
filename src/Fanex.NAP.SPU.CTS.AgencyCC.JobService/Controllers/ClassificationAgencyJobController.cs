namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency;
    using Microsoft.AspNetCore.Mvc;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    [Route("api/classificationAgencyJobs")]
    [ApiController]
    public class ClassificationAgencyJobController : ControllerBase
    {
        private readonly ISentryClient _sentryClient;
        private readonly IRealtimeClassificationAgencyJobService _realtimeClassification;
        private readonly IDailyNormalClassificationAgencyJobService _dailyNormalClassification;
        private readonly INormalAccountClassificationAgencyJobService _normalAccountClassification;
        private readonly INormalAccountInsertAgencyJobService _normalAccountInsert;

        public ClassificationAgencyJobController(
            ISentryClient sentryClient,
            IRealtimeClassificationAgencyJobService realtimeClassification,
            IDailyNormalClassificationAgencyJobService dailyNormalClassification,
            INormalAccountClassificationAgencyJobService normalAccountClassification,
            INormalAccountInsertAgencyJobService normalAccountInsert)
        {
            _sentryClient = sentryClient;
            _realtimeClassification = realtimeClassification;
            _dailyNormalClassification = dailyNormalClassification;
            _normalAccountClassification = normalAccountClassification;
            _normalAccountInsert = normalAccountInsert;
        }

        [HttpPost("scanningRealtimeClassificationAgency")]
        public async Task<bool> RealtingScanningClassificationAgency([FromForm] int batchExternalSize, [FromForm] int batchInternalSize)
        {
            return await _realtimeClassification.Execute(batchExternalSize, batchInternalSize);
        }

        [HttpPost("scanningDailyNormalClassificationAgency")]
        public async Task<bool> DailyScanningNormalClassificationAgency()
        {
            _sentryClient.CaptureMessage($"Daily Normal Agency Service is running at {DateTime.Now}");
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
    }
}
