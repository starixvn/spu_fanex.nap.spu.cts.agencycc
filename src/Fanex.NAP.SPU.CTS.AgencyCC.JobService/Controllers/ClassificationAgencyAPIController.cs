namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.TWRobot;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgencyAPI.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency.AffectDownlineCustomerClassification;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Route("api/classificationAgencyAPI")]
    [ApiController]
    public class ClassificationAgencyApiController : ControllerBase
    {
        private readonly INewCustomerClassificationAgencyJobService _newCustomerClassificationAgency;
        private readonly IAffectedDownlineAgencyJobService _affectedDownlineAgencyJobService;
        private readonly IRobotAgencyJobService _robotAgencyJobService;

        public ClassificationAgencyApiController(INewCustomerClassificationAgencyJobService newCustomerClassificationAgency
            , IAffectedDownlineAgencyJobService affectedDownlineAgencyJobService
            , IRobotAgencyJobService robotAgencyJobService)
        {
            _newCustomerClassificationAgency = newCustomerClassificationAgency;
            _affectedDownlineAgencyJobService = affectedDownlineAgencyJobService;
            _robotAgencyJobService = robotAgencyJobService;
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

        [HttpPost("insertTWRobotAgency")]
        public async Task<bool> InsertTWRobotAgency([FromBody] CustomerClassificationAgencyParameterModel<InsertTWAgencyRobotModel> model)
        {
            return await _robotAgencyJobService.Execute(model);
        }
    }
}
