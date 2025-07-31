namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IProblemAccountAgencyService
    {
        IEnumerable<long> RescanCustomerClassification(IEnumerable<RescanCustomerClassificationAgencyEntity> custRescans,
            FunctionId funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue, string serviceName);

        IEnumerable<long> InsertProblemAccountsAgencyPackage(IEnumerable<InsertProblemAccountEntity> problemAccounts,
            FunctionId funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue, string serviceName);

        ResultRemoveCustomerClassificationAgencyModel RemoveConsiderable(IEnumerable<RemoveProblemAccountEntity> problemAccounts
            , AgencyCCInputFlow inputFlow, bool isFromQueue);

        ResultRemoveCustomerClassificationAgencyModel RemoveProblemAccount(IEnumerable<RemoveProblemAccountEntity> problemAccounts, bool isFromQueue);

        int? GetCategoryByCategoryTypeName(string categoryTypeName);

        IEnumerable<ProblemAccountLosingAgencyEntity> GetProblemAccountLosingAgencies(IEnumerable<long> custIds);
    }

    public class ProblemAccountAgencyService : BaseCustomerClassificationAgencyService, IProblemAccountAgencyService
    {
        private readonly IProblemAccountAgencyDataAccess _paAgencyDataAccess; 

        public ProblemAccountAgencyService(
            IBaseCustomerClassificationAgencyDataAccess baseCCAgencyDataAccess,
            IProblemAccountAgencyDataAccess paAgencyDataAccess) : base(baseCCAgencyDataAccess) 
        {
            _paAgencyDataAccess = paAgencyDataAccess; 
        }

        public IEnumerable<long> RescanCustomerClassification(IEnumerable<RescanCustomerClassificationAgencyEntity> custRescans,
            FunctionId funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue, string serviceName)
        {
            var pas = custRescans.Select(x => new InsertProblemAccountEntity(x));

            return InsertProblemAccountsAgencyPackage(pas, funcId, ccInputFlow, isToQueue, serviceName);
        }

        public IEnumerable<long> InsertProblemAccountsAgencyPackage(IEnumerable<InsertProblemAccountEntity> problemAccounts,
            FunctionId funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue, string serviceName)
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
                    return insertPaResult.CustInfos.Select(x => x.CustID).Distinct();
                }
            }

            return Enumerable.Empty<long>();
        }

        public ResultRemoveCustomerClassificationAgencyModel RemoveConsiderable(IEnumerable<RemoveProblemAccountEntity> problemAccounts
            , AgencyCCInputFlow inputFlow, bool isFromQueue)
        {
            var modelRemove = new RemoveCustomerClassificationAgencyModel
            {
                CustClassificationJson = JsonConvert.SerializeObject(problemAccounts),
                IsFromQueue = isFromQueue,
                FromAction = (int)FromFlow.CTSServices
            };

            return RemoveCustomerClassificationAgency(inputFlow, modelRemove);
        }

        public ResultRemoveCustomerClassificationAgencyModel RemoveProblemAccount(IEnumerable<RemoveProblemAccountEntity> problemAccounts, bool isFromQueue)
        {
            var modelRemove = new RemoveCustomerClassificationAgencyModel
            {
                CustClassificationJson = JsonConvert.SerializeObject(problemAccounts),
                IsFromQueue = isFromQueue,
                FromAction = (int)FromFlow.CTSServices
            };

            return RemoveCustomerClassificationAgency(AgencyCCInputFlow.RemovePACategory, modelRemove);
        }

#pragma warning disable S125
        // not handle for SMA
        //public void InsertPALogicsAgency(IEnumerable<CustInfoEntity> customerInfo)
        //{
        //    if (customerInfo != null && customerInfo.Any())
        //    {
        //        var custPas = customerInfo.Where(x => x.IsPA).Distinct();

        //        CheckNewPAAssociationCustomerForRollback(JsonConvert.SerializeObject(custPas));

        //        UpdateBetLimitOfFlaggedCustomer(custPas.Select(x => x.CustID));
        //    }
        //}

        //private void RemovePALogics(IEnumerable<ResultRemoveCustomerClassificationEntity> customerInfo)
        //{
        //    if (customerInfo != null && customerInfo.Any())
        //    {
        //        var custIds = customerInfo.Where(c => c.IsCleanPA).Select(x => x.CustID).Distinct();

        //        UpdateBetLimitOfUnflaggedCustomer(custIds);
        //    }
        //}

        //private void UpdateBetLimitOfFlaggedCustomer(IEnumerable<long> custId)
        //{
        //    if (custId != null && custId.Any())
        //    {
        //        UpdateBetLimitOfFlaggedCustomer(string.Join(",", custId));
        //    }
        //}

        //private void UpdateBetLimitOfUnflaggedCustomer(IEnumerable<long> custId)
        //{
        //    if (custId != null && custId.Any())
        //    {
        //        UpdateBetLimitOfUnflaggedCustomer(string.Join(",", custId));
        //    }
        //}
#pragma warning disable S125

        public IEnumerable<ProblemAccountLosingAgencyEntity> GetProblemAccountLosingAgencies(IEnumerable<long> custIds)
        {
            if (custIds != null && custIds.Any())
            {
                var custIDList = string.Join(",", custIds);

                return _paAgencyDataAccess.GetProblemAccountLosingAgencies(custIDList);
            }

            return Enumerable.Empty<ProblemAccountLosingAgencyEntity>();
        }

        private IEnumerable<InsertProblemAccountEntity> CombinePAWithLosingPerformace(IEnumerable<InsertProblemAccountEntity> problemAccounts)
        {
            var custIDs = problemAccounts.Select(x => x.CustID)?.Distinct();

            var paLosing = GetProblemAccountLosingAgencies(custIDs);
            
            return MergePALosingClassification(problemAccounts, paLosing);
        }

        private IEnumerable<InsertProblemAccountEntity> MergePALosingClassification(IEnumerable<InsertProblemAccountEntity> problemAccounts, IEnumerable<ProblemAccountLosingAgencyEntity> paLosing)
        {
            if (problemAccounts != null && problemAccounts.Any())
            {
                return problemAccounts.GroupJoin(paLosing ?? Enumerable.Empty<ProblemAccountLosingAgencyEntity>(),
                                                 pa => pa.CustID,
                                                 pl => pl.CustId,
                                                (pa, pl) => new { pa, pl })
                                                .SelectMany(
                                                        group => group.pl.DefaultIfEmpty(),
                                                        (group, temp) => new InsertProblemAccountEntity
                                                        {
                                                            CTSCustID = group.pa.CTSCustID,
                                                            CustID = group.pa.CustID,
                                                            RoleID = group.pa.RoleID,
                                                            SubscriberID = group.pa.SubscriberID,
                                                            CategoryID = group.pa.CategoryID,
                                                            IsLicensee = group.pa.IsLicensee,
                                                            CreatedBy = group.pa.CreatedBy, 
                                                            CreatedDate = group.pa.CreatedDate,
                                                            Remark = group.pa.Remark,
                                                            IsMarkedDirectly = group.pa.IsMarkedDirectly,
                                                            IsFromAI = group.pa.IsFromAI,
                                                            IsFromCTS = group.pa.IsFromCTS,
                                                            IsFromTW = group.pa.IsFromTW, 
                                                          
                                                            RobotCounter = group.pa.RobotCounter, 
                                                            BetCount = temp?.BetCount,
                                                            TurnoverRM = temp?.TurnoverRM,
                                                            WinlossRM = temp?.WinlossRM,
                                                            WinlossStatus = temp?.WinlossStatus,
                                                            PerformanceTime = temp?.PerformanceTime
                                                        });
            }

            return Enumerable.Empty<InsertProblemAccountEntity>();
        }
    }
}
