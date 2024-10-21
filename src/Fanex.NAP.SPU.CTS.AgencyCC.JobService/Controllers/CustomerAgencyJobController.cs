namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CTSCustomerAgency;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Sentry;
    using System;
    using System.Threading.Tasks;

    [Route("api/TransformCustomerAgency")]
    [ApiController]
    public class CustomerAgencyJobController : ControllerBase
    {
        private readonly ISentryClient _sentryClient;
        private readonly ICTSCustomerAgencyJobService _ctsCustomerAgencyJob;

        public CustomerAgencyJobController(ISentryClient sentryClient, ICTSCustomerAgencyJobService ctsCustomerAgencyJob)
        {
            _sentryClient = sentryClient;
            _ctsCustomerAgencyJob = ctsCustomerAgencyJob;
        }

        [HttpPost("InactivateNewCustomersAgency")]
        public async Task<bool> InactivateNewCustomers([FromForm] int batchSize)
        {
            return await _ctsCustomerAgencyJob.InactivateNewCustomers(batchSize);
        }

        [HttpPost("TestWritelog")]
        public async Task<bool> TestWritelog()
        {
            _sentryClient.CaptureMessage($"Test Writelog Agency CC success {DateTimeOffset.Now}", Sentry.Protocol.SentryLevel.Info);

            return await Task.FromResult(true);
        }
    }
}
