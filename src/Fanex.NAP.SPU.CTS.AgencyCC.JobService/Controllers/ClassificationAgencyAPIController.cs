namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgencyAPI.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency.AffectDownlineCustomerClassification;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/classificationAgencyAPI")]
    [ApiController]
    public class ClassificationAgencyAPIController : ControllerBase
    {
        private readonly INewCustomerClassificationAgencyJobService _newCustomerClassificationAgency;

        public ClassificationAgencyAPIController(INewCustomerClassificationAgencyJobService newCustomerClassificationAgency)
        {
            _newCustomerClassificationAgency = newCustomerClassificationAgency;
        }

        [HttpPost("newCustomerClassificationAgency")]
        public async Task<bool> NewCustomerClassification([FromBody] CustomerClassificationAgencyParameterModel<NewCustomerClassificationAgencyModel> model)
        {
            return await _newCustomerClassificationAgency.Execute(model);
        }
    }
}
