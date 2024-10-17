namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;   
    using Newtonsoft.Json;
    using Sentry;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IProblemAccountAgencyService
    {
        IEnumerable<long> RescanCustomerClassification(IEnumerable<RescanCustomerClassificationAgencyEntity> custRescans, FunctionId funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue, string serviceName);

        IEnumerable<long> InsertProblemAccountsAgencyPackage(IEnumerable<InsertProblemAccountEntity> problemAccounts, FunctionId funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue, string serviceName);

        ResultRemoveCustomerClassificationAgencyModel RemoveProblemAccount(IEnumerable<RemoveProblemAccountEntity> problemAccounts, bool isFromQueue);
    }

    public class ProblemAccountAgencyService : BaseCustomerClassificationAgencyService, IProblemAccountAgencyService
    {
        public ProblemAccountAgencyService(
            IBaseCustomerClassificationAgencyDataAccess baseCCAgencyDataAccess,
            ISentryClient sentryClient) : base(baseCCAgencyDataAccess, sentryClient) { }

        public IEnumerable<long> RescanCustomerClassification(IEnumerable<RescanCustomerClassificationAgencyEntity> custRescans, FunctionId funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue, string serviceName)
        {
            var pas = custRescans.Select(x => new InsertProblemAccountEntity(x));

            return InsertProblemAccountsAgencyPackage(pas, funcId, ccInputFlow, isToQueue, serviceName);          
        }

        public IEnumerable<long> InsertProblemAccountsAgencyPackage(IEnumerable<InsertProblemAccountEntity> problemAccounts, FunctionId funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue, string serviceName)
        {
            if (problemAccounts != null && problemAccounts.Any())
            {
                var paClassification = CombinePAWithLosingPerformace(problemAccounts);

                var modelInsert = new InsertCustomerClassificationAgencyModel
                {
                    CustInfoJson = JsonConvert.SerializeObject(paClassification),
                    IsToQueue = isToQueue,
                    FromAction = (int)FromFlow.CTSServices
                };

                var insertPaResult = base.InsertCustomerClassification(ccInputFlow, modelInsert, serviceName);

                if (insertPaResult.CustInfos != null && insertPaResult.CustInfos.Any())
                {
                    var custUpdatePALogics = insertPaResult.CustInfos.Select(x => new CustInfoEntity { CustID = x.CustID, CTSCustID = x.CTSCustID, IsPA = x.IsPA });

                    InsertPALogicsAgency(custUpdatePALogics);

                    return insertPaResult.CustInfos.Select(x => x.CustID).Distinct();
                }
            }

            return Enumerable.Empty<long>();
        }

        public ResultRemoveCustomerClassificationAgencyModel RemoveProblemAccount(IEnumerable<RemoveProblemAccountEntity> problemAccounts, bool isFromQueue)
        {
            var modelRemove = new RemoveCustomerClassificationAgencyModel
            {
                CustClassificationJson = JsonConvert.SerializeObject(problemAccounts),
                IsFromQueue = isFromQueue,
                FromAction = (int)FromFlow.CTSWEB
            };

            var removePaResult = RemoveCustomerClassificationAgency(AgencyCCInputFlow.RemovePACategory, modelRemove);

            if (string.IsNullOrEmpty(removePaResult.ErrorMessage))
            {
                RemovePALogics(removePaResult.CustInfos);
            }            

            return removePaResult;
        }

        private void InsertPALogicsAgency(IEnumerable<CustInfoEntity> customerInfo)
        {
            if (customerInfo != null && customerInfo.Any())
            {
                var custPas = customerInfo.Where(x => x.IsPA).Distinct();

#pragma warning disable S125
                // not handle for SMA
                //CheckNewPAAssociationCustomerForRollback(JsonConvert.SerializeObject(custPas));
#pragma warning disable S125

                UpdateBetLimitOfFlaggedCustomer(custPas.Select(x => x.CustID));
            }
        }

        private void RemovePALogics(IEnumerable<ResultRemoveCustomerClassificationEntity> customerInfo)
        {
            if (customerInfo != null && customerInfo.Any())
            {
                var custIds = customerInfo.Where(c => c.IsCleanPA).Select(x => x.CustID).Distinct();

                UpdateBetLimitOfUnflaggedCustomer(custIds);
            }
        }

        private void UpdateBetLimitOfFlaggedCustomer(IEnumerable<long> custId)
        {
            if (custId != null && custId.Any())
            {
                UpdateBetLimitOfFlaggedCustomer(string.Join(",", custId));
            }
        }

        private void UpdateBetLimitOfUnflaggedCustomer(IEnumerable<long> custId)
        {
            if (custId != null && custId.Any())
            {
                UpdateBetLimitOfUnflaggedCustomer(string.Join(",", custId));
            }
        }       

        private IEnumerable<InsertProblemAccountEntity> CombinePAWithLosingPerformace(IEnumerable<InsertProblemAccountEntity> problemAccounts)
        {
            return problemAccounts.Select(x => {
                var y = x;
                y.WinlossStatus = 2;
                y.TurnoverRM = 0;
                y.WinlossRM = 0;
                y.BetCount = 0;
                y.PerformanceTime = null;
                return y;
            });
        }
    }
}
