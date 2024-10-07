namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert.Model;
    using Sentry;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IProblemAccountAgencyService
    {
        IEnumerable<long> RescanCustomerClassification(IEnumerable<RescanCustomerClassificationAgencyEntity> custRescans, FunctionIdAgency funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue, string serviceName);
    }

    public class ProblemAccountAgencyService : BaseCustomerClassificationAgencyService, IProblemAccountAgencyService
    {
        public ProblemAccountAgencyService(
            IBaseCustomerClassificationAgencyDataAccess baseCCAgencyDataAccess,
            ISentryClient sentryClient) : base(baseCCAgencyDataAccess, sentryClient) { }

        public IEnumerable<long> RescanCustomerClassification(IEnumerable<RescanCustomerClassificationAgencyEntity> custRescans, FunctionIdAgency funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue, string serviceName)
        {
            var pas = custRescans.Select(x => new InsertProblemAccountAgencyModel(x));

            return InsertProblemAccountsAgencyPackage(pas, funcId, ccInputFlow, isToQueue, serviceName);
        }

        #region private method
        private IEnumerable<long> InsertProblemAccountsAgencyPackage(IEnumerable<InsertProblemAccountAgencyModel> problemAccounts, FunctionIdAgency funcId, AgencyCCInputFlow ccInputFlow, bool isToQueue, string serviceName)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
