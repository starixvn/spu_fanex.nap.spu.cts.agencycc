namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    public interface IProblemWLClassificationAgencyService
    {
        IProblemWLClassificationAgencyService InsertPAWLClassificationAgency(int batchSize, IEnumerable<ProblemAccountLosingAgencyEntity> paWLClassification);

#pragma warning disable S125
        // not handle SMA
        //IProblemWLClassificationAgencyService InsertPALogicAgency();
#pragma warning disable S125

        bool PushCCToExternal(PushDataToExternalModel paras);
    }

    public class ProblemWLClassificationAgencyService : IProblemWLClassificationAgencyService
    {
        private readonly IProblemClassificationAgencyDataAccess _dataAccess;
        private readonly ICustClassToMainDBService _custClassToMainDB;

        private IEnumerable<PAWLInsertAgencyEntity> _paWLs;

        public ProblemWLClassificationAgencyService(
            IProblemClassificationAgencyDataAccess dataAccess,
            ICustClassToMainDBService custClassToMainDB)
        {
            _dataAccess = dataAccess;
            _custClassToMainDB = custClassToMainDB;
            _paWLs = new List<PAWLInsertAgencyEntity>();
        }

        public IProblemWLClassificationAgencyService InsertPAWLClassificationAgency(int batchSize, IEnumerable<ProblemAccountLosingAgencyEntity> paWLClassification)
        {
            if (paWLClassification.IsNotNullOrEmpty())
            {
                var paJson = JsonConvert.SerializeObject(paWLClassification);

                _paWLs = _dataAccess.InsertProblemAccountWLPerformance(paJson);
            }

            return this;
        }

#pragma warning disable S125
        // not handle SMA
        //public IProblemWLClassificationAgencyService InsertPALogicAgency()
        //{
        //    if (_paWLs.IsNotNullOrEmpty())
        //    {
        //        var paToInsertLogic = _paWLs.Where(x => !x.IsRobot).Select(x => new CustInfoEntity
        //        {
        //            CTSCustID = x.CTSCustID,
        //            CustID = x.CTSCustID
        //        });

        //        _paAgencyService.InsertPALogicsAgency(paToInsertLogic);
        //    }

        //    return this;
        //}
#pragma warning disable S125


        public bool PushCCToExternal(PushDataToExternalModel paras)
        {
            if (_paWLs.IsNotNullOrEmpty())
            {
                var custIDs = _paWLs.Select(c => c.CustID).Distinct();

                return _custClassToMainDB.PushExternalPackageByCustId(custIDs, paras);
            }

            return true;
        }
    }
}
