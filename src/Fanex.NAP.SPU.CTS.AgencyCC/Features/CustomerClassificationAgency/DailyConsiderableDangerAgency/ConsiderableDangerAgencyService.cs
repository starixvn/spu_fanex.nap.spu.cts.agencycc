namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System.Collections.Generic;
    using System.Linq;

    public interface IConsiderableDangerAgencyService
    {
        IConsiderableDangerAgencyService UpdateConsiderableDangerQueueMaxID();

        ConsiderableDangerRecheckModel GetConsiderableDangerQueue(int batchSize);

        IEnumerable<DetectConsiderableEntity> DetectConsiderable(IEnumerable<long> CustIDs);

        IConsiderableDangerAgencyService CompleteConsiderableQueue(long lastID);

        IConsiderableDangerAgencyService InsertConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts, 
            PushDataToExternalModel pushDataExternal, 
            Constants.AgencyCCInputFlow inputFlow);

        IConsiderableDangerAgencyService RemoveConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts);
    }

    public class ConsiderableDangerAgencyService : IConsiderableDangerAgencyService
    {
        private readonly IConsiderableDangerAgencyDataAccess _dataAccess;
        private readonly IProblemAccountAgencyService _problemAccountService;
        private readonly ICustClassToMainDBService _custClassToMainDBService;

        public ConsiderableDangerAgencyService(IConsiderableDangerAgencyDataAccess dataAccess,
            IProblemAccountAgencyService problemAccountService,
            ICustClassToMainDBService custClassToMainDBService)
        {
            _dataAccess = dataAccess;
            _problemAccountService = problemAccountService;
            _custClassToMainDBService = custClassToMainDBService;
        }

        public IConsiderableDangerAgencyService UpdateConsiderableDangerQueueMaxID()
        {
            _dataAccess.UpdateConsiderableDangerQueueMaxID();

            return this;
        }

        public ConsiderableDangerRecheckModel GetConsiderableDangerQueue(int batchSize)
        {
            return _dataAccess.GetConsiderableDangerQueue(batchSize);
        }

        public IEnumerable<DetectConsiderableEntity> DetectConsiderable(IEnumerable<long> CustIDs)
        {
            return _dataAccess.DetectConsiderable(CustIDs);
        }

        public IConsiderableDangerAgencyService CompleteConsiderableQueue(long lastID)
        {
            _dataAccess.CompleteConsiderableQueue(lastID);

            return this;
        }

        public IConsiderableDangerAgencyService InsertConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts,
            PushDataToExternalModel pushDataExternal,
            Constants.AgencyCCInputFlow inputFlow)
        {           
            var problems = considerableCusts.Select(x => new InsertProblemAccountEntity(x));

            var result = _problemAccountService.InsertProblemAccountsAgencyPackage(problems, 
                Constants.FunctionId.Default,
                inputFlow, 
                false, 
                nameof(ConsiderableDangerAgencyService));

            _custClassToMainDBService.PushExternalProblemAcountPackage(result, pushDataExternal);

            return this;
        }

        public IConsiderableDangerAgencyService RemoveConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts)
        {
            var considerableAccounts = considerableCusts.Select(x => new RemoveProblemAccountEntity
            {
                CustID = x.CustID,
                CTSCustID = x.CTSCustID,
                RoleID = x.RoleID,
                SubscriberID = x.SubscriberID,               
                IsMarkedDirectly = true,              
                CreatedBy = Constants.StarixITId
            });

            _problemAccountService.RemoveConsiderable(considerableAccounts, Constants.AgencyCCInputFlow.RemoveConderable , false);

            return this;
        }
    }
}
