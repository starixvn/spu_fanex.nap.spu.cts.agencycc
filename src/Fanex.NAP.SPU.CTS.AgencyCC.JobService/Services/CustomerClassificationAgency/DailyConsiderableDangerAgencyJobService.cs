namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IDailyConsiderableDangerAgencyJobService
    {
        Task<bool> Execute(int batchSize);
    }

    public class DailyConsiderableDangerAgencyJobService : IDailyConsiderableDangerAgencyJobService
    {
        private readonly IConsiderableDangerAgencyService _considerableService;
      
        public DailyConsiderableDangerAgencyJobService(IConsiderableDangerAgencyService considerableService)
        {
            _considerableService = considerableService;            
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

            // Prepare data
            UpdateConsiderableDangerQueue();

            do
            {
                var considerableQueue = GetConsiderableDangerQueue(batchSize);

                isContinue = considerableQueue.LastID > 0;

                if (isContinue)
                {
                    var detectConsiderable = DetectConsiderableCustomers(considerableQueue);

                    // Process customers for insertion
                    var customersToInsert = BuildCustomersForInsertion(considerableQueue, detectConsiderable);
                    InsertConsiderableCategories(customersToInsert);

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

        private IEnumerable<ConsiderableDangerRecheckEntity> BuildCustomersForInsertion(
            ConsiderableDangerRecheckModel considerableQueue, 
            IEnumerable<DetectConsiderableEntity> detectConsiderable)
        {
            var customersToInsert = new List<ConsiderableDangerRecheckEntity>();

            // Add WL customers
            customersToInsert.AddRange(AddConsiderableRecheckWL(considerableQueue));

            // Add ratio customers
            customersToInsert.AddRange(AddConsiderableRecheckRatio(
                considerableQueue.RecheckRatioCusts, 
                detectConsiderable));

            return customersToInsert;
        }

        private void InsertConsiderableCategories(IEnumerable<ConsiderableDangerRecheckEntity> customersToInsert)
        {
            _considerableService.InsertConsiderableCategory(customersToInsert);
        }

        private IEnumerable<ConsiderableDangerRecheckEntity> BuildCustomersForRemoval(
            ConsiderableDangerRecheckModel considerableQueue, 
            IEnumerable<DetectConsiderableEntity> detectConsiderable)
        {
            var customersPassingConsiderableRule = detectConsiderable.Where(x => x.IsPassRuleConsiderable);
            
            return AddConsiderableRecheckRatio(
                considerableQueue.RecheckRatioCusts, 
                customersPassingConsiderableRule);
        }

        private void RemoveConsiderableCategories(IEnumerable<ConsiderableDangerRecheckEntity> customersToRemove)
        {
            _considerableService.RemoveConsiderableCategory(customersToRemove);
        }

        private void CompleteConsiderableQueue(ConsiderableDangerRecheckModel considerableQueue)
        {
            _considerableService.CompleteConsiderableQueue(considerableQueue.LastID);
        }

        private IEnumerable<ConsiderableDangerRecheckEntity> AddConsiderableRecheckWL(ConsiderableDangerRecheckModel data)
        {
            if (data != null && data.RecheckWLCusts.IsNotNullOrEmpty())
            {
                return data.RecheckWLCusts;
            }

            return Enumerable.Empty<ConsiderableDangerRecheckEntity>();
        }

        private IEnumerable<ConsiderableDangerRecheckEntity> AddConsiderableRecheckRatio(IEnumerable<ConsiderableDangerRecheckEntity> recheckRatioCusts,
            IEnumerable<DetectConsiderableEntity> detectConsiderables)
        {
            var recheckRatioCustsDict = recheckRatioCusts.ToDictionary(d => d.CustID);

            var result = detectConsiderables.Where(x => x.IsPassRuleRatio)
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
