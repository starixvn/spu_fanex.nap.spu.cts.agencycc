namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IRescanCustClassificationAgencyService
    {
        IRescanCustClassificationAgencyService Start();

        IRescanCustClassificationAgencyService GetAgencyCusts(IEnumerable<long> custIds);

        bool RescanCustomerClassificationAgency(IEnumerable<long> custIds, RescanParametersModel paras);

        bool PushExternalProblemAccounts(IEnumerable<long> custIds, PushDataToExternalModel paras);
    }

    public class RescanCustClassificationAgencyService : ProblemAccountAgencyService, IRescanCustClassificationAgencyService
    {
        private readonly IRescanCustClassificationAgencyDataAccess _dataAccess;
        private readonly ICustClassToMainDBService _custClassToMainDBService;

        private List<long> smaCustIds;
        private bool isSuccess;

        public RescanCustClassificationAgencyService(
            IBaseCustomerClassificationAgencyDataAccess baseCCAgencyDataAccess,
            IProblemAccountAgencyDataAccess paAgencyDataAccess,
            IRescanCustClassificationAgencyDataAccess dataAccess,
            ICustClassToMainDBService custClassToMainDBService) : base(baseCCAgencyDataAccess, paAgencyDataAccess)
        {
            _dataAccess = dataAccess;
            _custClassToMainDBService = custClassToMainDBService;
        }

        public IRescanCustClassificationAgencyService Start()
        {
            smaCustIds = new List<long>();

            isSuccess = true;

            return this;
        }

        public IRescanCustClassificationAgencyService GetAgencyCusts(IEnumerable<long> custIds)
        {
            var data = _dataAccess.GetRoleCustomer(string.Join(",", custIds));

            if (data != null && data.Any())
            {
#pragma warning disable S2589 // Boolean expressions should not be gratuitous
                smaCustIds = data.Where(c => c.RoleID == (int)CustomerRole.Agent
                                          || c.RoleID == (int)CustomerRole.Master
                                          || c.RoleID == (int)CustomerRole.Super).Select(c => c.CustID)?.ToList();
#pragma warning restore S2589 // Boolean expressions should not be gratuitous

            }

            return this;
        }

        public bool RescanCustomerClassificationAgency(IEnumerable<long> custIds, RescanParametersModel paras)
        {
            Start().GetAgencyCusts(custIds);

            var custRescans = GetRescanCustomerClassificationAgency(string.Join(",", custIds));
            var custIdsToRescan = new List<long>();

            if (custRescans.CustInfos.IsNotNullOrEmpty())
            {
                var groupingCustRescans = custRescans.CustInfos
                    .GroupBy(x => x.InputFlowID)
                    .Select(g => new { InputFlowID = g.Key, CustRescans = g.Select(x => x) });

                foreach (var group in groupingCustRescans)
                {
                    switch ((AgencyCCInputFlow)group.InputFlowID)
                    {                                
                        case AgencyCCInputFlow.RescanNormal:
                            var custIdsRescan = group.CustRescans.Select(x => x.CustID);
                            PreNormalClassification(custIdsRescan, paras);
                            custIdsToRescan.AddRange(custIdsRescan);
                            break;                    
                        case AgencyCCInputFlow.RecanConsiderable:
                            var resultRescanConsiderable = RescanConsiderableDanger(group.CustRescans, paras, (AgencyCCInputFlow)group.InputFlowID);
                            custIdsToRescan.AddRange(resultRescanConsiderable);
                            break;                       
                        default:
                            var resultCustRescans = RescanProblemAccount(group.CustRescans, paras, (AgencyCCInputFlow)group.InputFlowID);
                            custIdsToRescan.AddRange(resultCustRescans);
                            break;
                    }
                }
            }

            var custToRemovePA = smaCustIds.Except(custIdsToRescan);

            if (custToRemovePA.IsNotNullOrEmpty())
            {
                isSuccess = _custClassToMainDBService.PushExternalProblemAcountPackage(custToRemovePA.Distinct(), paras.PushDataExternal);
            }

            return isSuccess;
        }

        public bool PushExternalProblemAccounts(IEnumerable<long> custIds, PushDataToExternalModel paras)
        {
            if (custIds.Any())
            {
                return _custClassToMainDBService.PushExternalProblemAcountPackage(custIds.Distinct(), paras);
            }

            return true;
        }

        #region private method
        private void PreNormalClassification(IEnumerable<long> custNormals, RescanParametersModel paras)
        {
            var custIDToString = string.Join(",", custNormals);

            _dataAccess.PreNormalClassification(custIDToString, paras.FunctionId, paras.RescanNormalType);
        }

        private IEnumerable<long> RescanProblemAccount(IEnumerable<RescanCustomerClassificationAgencyEntity> custProblems, RescanParametersModel paras, AgencyCCInputFlow ccInputFlow)
        {
            var resultCustRescans = base.RescanCustomerClassification(custProblems, paras.FunctionId, ccInputFlow, paras.IsToQueue, paras.ServiceName);

            PushExternalProblemAccounts(resultCustRescans, paras.PushDataExternal);

            return resultCustRescans;
        }

        private IEnumerable<long> RescanConsiderableDanger(IEnumerable<RescanCustomerClassificationAgencyEntity> custConsiderable, RescanParametersModel paras, AgencyCCInputFlow ccInputFlow)
        {
            var resultCustRescans = base.RescanCustomerClassification(custConsiderable, paras.FunctionId, ccInputFlow, false, paras.ServiceName);

            PushExternalProblemAccounts(resultCustRescans, paras.PushDataExternal);

            return resultCustRescans;
        }
        #endregion
    }
}
