namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;
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

        bool InsertConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts, 
            PushDataToExternalModel pushDataExternal, 
            Constants.AgencyCCInputFlow inputFlow);

        IConsiderableDangerAgencyService RemoveConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts, RescanParametersModel rescanParameter);
    }

    public class ConsiderableDangerAgencyService : IConsiderableDangerAgencyService
    {
        private readonly IConsiderableDangerAgencyDataAccess _dataAccess;
        private readonly IProblemAccountAgencyService _problemAccountService;
        private readonly ICustClassToMainDBService _custClassToMainDBService;
        private readonly IRescanCustClassificationAgencyService _rescanCustClassificationService;

        public ConsiderableDangerAgencyService(IConsiderableDangerAgencyDataAccess dataAccess,
            IProblemAccountAgencyService problemAccountService,
            ICustClassToMainDBService custClassToMainDBService,
            IRescanCustClassificationAgencyService rescanCustClassificationService)
        {
            _dataAccess = dataAccess;
            _problemAccountService = problemAccountService;
            _custClassToMainDBService = custClassToMainDBService;
            _rescanCustClassificationService = rescanCustClassificationService;
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

        public bool InsertConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts,
            PushDataToExternalModel pushDataExternal,
            Constants.AgencyCCInputFlow inputFlow)
        {           
            var problems = considerableCusts.Select(x => new InsertProblemAccountEntity(x));

            var result = _problemAccountService.InsertProblemAccountsAgencyPackage(problems, 
                Constants.FunctionId.Default,
                inputFlow, 
                false, 
                nameof(ConsiderableDangerAgencyService));

            return _custClassToMainDBService.PushExternalProblemAcountPackage(result, pushDataExternal);
        }

        public IConsiderableDangerAgencyService RemoveConsiderableCategory(IEnumerable<ConsiderableDangerRecheckEntity> considerableCusts, RescanParametersModel rescanParameter)
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

            var result = _problemAccountService.RemoveConsiderable(considerableAccounts, Constants.AgencyCCInputFlow.RemoveConderable , false);
            
            if (result != null && string.IsNullOrEmpty(result.ErrorMessage) && result.CustInfos.IsNotNullOrEmpty())
            {
                _rescanCustClassificationService.RescanCustomerClassificationAgency(result.CustInfos.Select(c => c.CustID).Distinct(), rescanParameter);
            }

            return this;
        }
    }
}
