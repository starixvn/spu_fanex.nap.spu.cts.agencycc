namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System;  
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IDailyConsiderableDangerAgencyJobService
    {
        Task<bool> Execute(int batchSize);
    }

    public class DailyConsiderableDangerAgencyJobService : IDailyConsiderableDangerAgencyJobService
    {
        private readonly IConsiderableDangerAgencyService _considerableService;
        private readonly IAppSettings _appSettings;

        public DailyConsiderableDangerAgencyJobService(IConsiderableDangerAgencyService considerableService,
            IAppSettings appSettings)
        {
            _considerableService = considerableService;
            _appSettings = appSettings;
        }

        public Task<bool> Execute(int batchSize)
        {
            try
            {
                if (batchSize > 0)
                {
                    JobRun(batchSize);
                }
                else
                {
                    Utilities.LogSentryError(nameof(DailyConsiderableDangerAgencyJobService), "InValid Batch size", (batchSize), new Exception());
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(nameof(DailyConsiderableDangerAgencyJobService), ex.Message, string.Empty, ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private void JobRun(int batchSize) 
        {
            var isContinue = true;
            var pushDataExternal = new PushDataToExternalModel
            {
                RoleGroup = RoleGroupType.Agency,
                ExternalBatchSize = batchSize,
                IsUpdateDangerToMainDB = false,
                IsWriteLog = _appSettings.ProblemAccountIsWriteLog,
                ServiceName = nameof(DailyConsiderableDangerAgencyJobService)
            };

            // Prepare data
            UpdateConsiderableDangerQueue();

            do
            {
                var considerableQueue = GetConsiderableDangerQueue(batchSize);

                isContinue = considerableQueue.LastID > 0;

                if (isContinue)
                {
                    var detectConsiderable = DetectConsiderableCustomers(considerableQueue);

                    // Process customers for insertion with WL
                    var custInsertWithWL = BuildCustomersForInsertWithWL(considerableQueue);
                    _considerableService.InsertConsiderableCategory(custInsertWithWL, pushDataExternal, AgencyCCInputFlow.RecanConsiderable);

                    // Process customers for insertion with not exist CD
                    var custInsertWithNotExistCD = BuildCustomersForInsertWithDetectCD(considerableQueue, detectConsiderable, false);
                    _considerableService.InsertConsiderableCategory(custInsertWithNotExistCD, pushDataExternal, AgencyCCInputFlow.InsertConderable);

                    // Process customers for insertion with exist CD
                    var custInsertWithExistCD = BuildCustomersForInsertWithDetectCD(considerableQueue, detectConsiderable, true);
                    _considerableService.InsertConsiderableCategory(custInsertWithExistCD, pushDataExternal, AgencyCCInputFlow.RecanConsiderable);

                    // Process customers for removal
                    var customersToRemove = BuildCustomersForRemoval(considerableQueue, detectConsiderable);
                    RemoveConsiderableCategories(customersToRemove);                    

                    // Complete the queue processing
                    CompleteConsiderableQueue(considerableQueue);
                }
            } while (isContinue);
        }

        private void UpdateConsiderableDangerQueue()
        {
            _considerableService.UpdateConsiderableDangerQueueMaxID();
        }

        private ConsiderableDangerRecheckModel GetConsiderableDangerQueue(int batchSize)
        {
            return _considerableService.GetConsiderableDangerQueue(batchSize);
        }

        private IEnumerable<DetectConsiderableEntity> DetectConsiderableCustomers(ConsiderableDangerRecheckModel considerableQueue)
        {
            var customerIds = considerableQueue.RecheckRatioCusts.Select(x => x.CustID);
            return _considerableService.DetectConsiderable(customerIds);
        }

        private IEnumerable<ConsiderableDangerRecheckEntity> BuildCustomersForInsertWithWL(ConsiderableDangerRecheckModel considerableQueue)
        {
            var customersToInsert = new List<ConsiderableDangerRecheckEntity>();

            if (considerableQueue != null && considerableQueue.RecheckWLCusts.IsNotNullOrEmpty())
            {
                customersToInsert.AddRange(considerableQueue.RecheckWLCusts);
            }
          
            return customersToInsert;
        }

        private IEnumerable<ConsiderableDangerRecheckEntity> BuildCustomersForInsertWithDetectCD(
            ConsiderableDangerRecheckModel considerableQueue,
            IEnumerable<DetectConsiderableEntity> detectConsiderable,
            bool isExitingCD)
        {
            var customersToInsert = new List<ConsiderableDangerRecheckEntity>();

            var custDetectCD = detectConsiderable.Where(x => x.IsExistingCD == isExitingCD);
           
            customersToInsert.AddRange(AddConsiderableRecheckRatio(considerableQueue.RecheckRatioCusts, custDetectCD, true));

            return customersToInsert;
        }              

        private IEnumerable<ConsiderableDangerRecheckEntity> BuildCustomersForRemoval(
            ConsiderableDangerRecheckModel considerableQueue, 
            IEnumerable<DetectConsiderableEntity> detectConsiderable)
        {
            var custPassIsExitingCD = detectConsiderable.Where(x => x.IsExistingCD);
            
            return AddConsiderableRecheckRatio(considerableQueue.RecheckRatioCusts, custPassIsExitingCD, false);
        }

        private void RemoveConsiderableCategories(IEnumerable<ConsiderableDangerRecheckEntity> customersToRemove)
        {
            _considerableService.RemoveConsiderableCategory(customersToRemove);
        }

        private void CompleteConsiderableQueue(ConsiderableDangerRecheckModel considerableQueue)
        {
            _considerableService.CompleteConsiderableQueue(considerableQueue.LastID);
        }
       
        private IEnumerable<ConsiderableDangerRecheckEntity> AddConsiderableRecheckRatio(IEnumerable<ConsiderableDangerRecheckEntity> recheckRatioCusts,
            IEnumerable<DetectConsiderableEntity> detectConsiderables,
            bool isPassRuleRatio)
        {
            var recheckRatioCustsDict = recheckRatioCusts.ToDictionary(d => d.CustID);

            var result = detectConsiderables.Where(x => x.IsPassRuleRatio == isPassRuleRatio)
                .Select(x => {
                    var cust = new ConsiderableDangerRecheckEntity();

                    if (recheckRatioCustsDict.TryGetValue(x.CustID, out var recheckRatio))
                    {
                        cust.CustID = recheckRatio.CustID;
                        cust.CTSCustID = recheckRatio.CTSCustID;
                        cust.CategoryID = recheckRatio.CategoryID;
                        cust.RoleID = recheckRatio.RoleID;
                        cust.SubscriberID = recheckRatio.SubscriberID;  
                        cust.IsLicensee = recheckRatio.IsLicensee;                             
                    }
                    return cust;
                }).Where(x=>x.CustID != 0);

            return result;
        }

    }
}
