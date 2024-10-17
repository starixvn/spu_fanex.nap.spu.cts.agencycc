namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgencyAPI.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency.AffectDownlineCustomerClassification;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/classificationAgencyAPI")]
    [ApiController]
#pragma warning disable S101 // Types should be named in PascalCase
    public class ClassificationAgencyAPIController : ControllerBase
#pragma warning restore S101 // Types should be named in PascalCase
    {
        private readonly INewCustomerClassificationAgencyJobService _newCustomerClassificationAgency;
        private readonly IAffectedDownlineAgencyJobService _affectedDownlineAgencyJobService;

        public ClassificationAgencyAPIController(INewCustomerClassificationAgencyJobService newCustomerClassificationAgency, IAffectedDownlineAgencyJobService affectedDownlineAgencyJobService)
        {
            _newCustomerClassificationAgency = newCustomerClassificationAgency;
            _affectedDownlineAgencyJobService = affectedDownlineAgencyJobService;
        }

        [HttpPost("newCustomerClassificationAgency")]
        public async Task<bool> NewCustomerClassification([FromBody] CustomerClassificationAgencyParameterModel<NewCustomerClassificationAgencyModel> model)
        {
            return await _newCustomerClassificationAgency.Execute(model);
        }

        [HttpPost("affectDownlineAgency")]
        public async Task<bool> AffectDownlineAgency([FromBody] CustClassificationFromQueueModel agencyInfos)
        {
            return await _affectedDownlineAgencyJobService.Execute(agencyInfos);
        }
    }
}
