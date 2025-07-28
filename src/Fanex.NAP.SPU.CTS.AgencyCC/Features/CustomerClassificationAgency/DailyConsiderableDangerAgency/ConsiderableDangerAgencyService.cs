namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System.Collections.Generic;
    using System.Linq;

    public interface IConsiderableDangerAgencyService
    {
        IConsiderableDangerAgencyService UpdateConsiderableDangerQueueMaxID();

        ConsiderableDangerRecheckModel GetConsiderableDangerQueue(int batchSize);

        IEnumerable<DetectConsiderableEntity> DetectConsiderable(IEnumerable<long> CustIDs);

        IConsiderableDangerAgencyService CompleteConsiderableQueue(long lastID);

        IConsiderableDangerAgencyService InsertConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts);

        IConsiderableDangerAgencyService RemoveConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts);
    }

    public class ConsiderableDangerAgencyService : IConsiderableDangerAgencyService
    {
        private readonly IConsiderableDangerAgencyDataAccess _dataAccess;
        private readonly IProblemAccountAgencyService _problemAccountService;

        public ConsiderableDangerAgencyService(IConsiderableDangerAgencyDataAccess dataAccess,
            IProblemAccountAgencyService problemAccountService)
        {
            _dataAccess = dataAccess;
            _problemAccountService = problemAccountService;
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

        public IConsiderableDangerAgencyService InsertConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts)
        {           
            var problems = considerableCusts.Select(x => new InsertProblemAccountEntity(x));

            _problemAccountService.InsertProblemAccountsAgencyPackage(problems, 
                Constants.FunctionId.Default, 
                Constants.AgencyCCInputFlow.InsertConderable, 
                false, 
                nameof(ConsiderableDangerAgencyService));

            return this;
        }

        public IConsiderableDangerAgencyService RemoveConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts)
        {
            var probAccounts = considerableCusts.Select(x => new RemoveProblemAccountEntity
            {
                CustID = x.CustID,
                CTSCustID = x.CTSCustID,
                RoleID = x.RoleID,
                SubscriberID = x.SubscriberID,               
                IsMarkedDirectly = true,              
                CreatedBy = Constants.StarixITId
            });

            _problemAccountService.RemoveProblemAccount(probAccounts, false);

            return this;
        }
    }
}
