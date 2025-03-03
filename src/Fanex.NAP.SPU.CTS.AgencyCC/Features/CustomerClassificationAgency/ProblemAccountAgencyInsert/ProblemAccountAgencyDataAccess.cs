namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert
{
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public interface IProblemAccountAgencyDataAccess
    {
        IEnumerable<ProblemAccountLosingAgencyEntity> GetProblemAccountLosingAgencies(string custIDList);
    }

    public class ProblemAccountAgencyDataAccess : IProblemAccountAgencyDataAccess
    {
        private readonly IDynamicRepository repository;

        public ProblemAccountAgencyDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<ProblemAccountLosingAgencyEntity> GetProblemAccountLosingAgencies(string custIDList) 
        {
            var criteria = new ProblemAccountLosingAgencyGetCriteria()
            {
                CustIDList = custIDList
            };

            return repository.Fetch<ProblemAccountLosingAgencyEntity>(criteria);
        }
    }
}
