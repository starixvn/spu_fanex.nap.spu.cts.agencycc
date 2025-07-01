namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.TWRobot;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgencyAPI.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.DataLogTracking;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IRobotAgencyJobService
    {
        Task<bool> Execute(CustomerClassificationAgencyParameterModel<InsertTWAgencyRobotModel> model);
    }

    public class RobotAgencyJobService : IRobotAgencyJobService
    {
        private readonly string _serviceName = "RobotInsertAgency";
        private readonly IProblemAccountAgencyService _problemAccountAgencyService;
        private readonly ICustClassToMainDBService _custClassToMainDBService;
        private readonly IDataLogTrackingServices _dataLogTrackingServices;
        private readonly IAppSettings _appSettings;

        public RobotAgencyJobService(
            IProblemAccountAgencyService problemAccountAgencyService,
            ICustClassToMainDBService custClassToMainDBService,
            IDataLogTrackingServices dataLogTrackingServices,
            IAppSettings appSettings)
        {
            _problemAccountAgencyService = problemAccountAgencyService;
            _custClassToMainDBService = custClassToMainDBService;
            _dataLogTrackingServices = dataLogTrackingServices;            
            _appSettings = appSettings;
        }

        public Task<bool> Execute(CustomerClassificationAgencyParameterModel<InsertTWAgencyRobotModel> model)
        {
            try
            {
                return Task.FromResult(JobRun(model.ExternalBatchSize, model.Data));
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex);

                _dataLogTrackingServices.LogErrorData(ex, _serviceName);

                return Task.FromResult(false);
            }
        }

        private bool JobRun(int externalBatchSize, InsertTWAgencyRobotModel robot)
        {
            var pushDataExternal = new PushDataToExternalModel
            {
                RoleGroup = RoleGroupType.Agency,
                IsUpdateDangerToMainDB = false, // not update for sma
                ExternalBatchSize = externalBatchSize,
                ServiceName = _serviceName,
                IsSyncAFCData = _appSettings.IsSyncAFCData,
                AFCInsertAccountURL = _appSettings.AFCInsertAccountURL,
                AFCDeleteAccountURL = _appSettings.AFCDeleteAccountURL
            };

            var robotAgencyCategoryID = _problemAccountAgencyService.GetCategoryByCategoryTypeName(CATEGORYTYPE_AGENCY_CATEID_ROBOT);

            var insertedPA = robot.Robots.Select(x => new InsertProblemAccountEntity(x, robotAgencyCategoryID));

            var result = _problemAccountAgencyService.InsertProblemAccountsAgencyPackage(insertedPA, 
                FunctionId.RobotUserJob,
                AgencyCCInputFlow.InsertPACategory, 
                false, 
                _serviceName);

            return _custClassToMainDBService.PushExternalProblemAcountPackage(result, pushDataExternal);
        }
    }
}
