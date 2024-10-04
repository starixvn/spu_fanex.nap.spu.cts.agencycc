namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CTSCustomerAgency;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/TransformCustomerAgency")]
    [ApiController]
    public class CustomerAgencyJobController : ControllerBase
    {
        private readonly ICTSCustomerAgencyJobService _ctsCustomerAgencyJob;

        public CustomerAgencyJobController(ICTSCustomerAgencyJobService ctsCustomerAgencyJob)
        {
            _ctsCustomerAgencyJob = ctsCustomerAgencyJob;
        }

        [HttpPost("InactivateNewCustomersAgency")]
        public async Task<bool> InactivateNewCustomers([FromForm] int batchSize)
        {
            return await _ctsCustomerAgencyJob.InactivateNewCustomers(batchSize);
        }
    }
}
