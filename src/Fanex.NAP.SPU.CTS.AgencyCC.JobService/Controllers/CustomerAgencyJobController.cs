namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CTSCustomerAgency;
    using Fanex.NAP.SPU.CTS.Shares;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Route("api/TransformCustomerAgency")]
    [ApiController]
    public class CustomerAgencyJobController : ControllerBase
    {
        private readonly ICtsCustomerAgencyJobService _ctsCustomerAgencyJob;

        public CustomerAgencyJobController(ICtsCustomerAgencyJobService ctsCustomerAgencyJob)
        {
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
            LogExtension.LogInfo($"Test Writelog Agency CC success {DateTimeOffset.Now}");

            return await Task.FromResult(true);
        }
    }
}
