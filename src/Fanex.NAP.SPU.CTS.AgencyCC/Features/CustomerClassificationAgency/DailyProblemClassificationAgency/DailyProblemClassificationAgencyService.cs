namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency
{
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;
    using System.Collections.Generic;
    using System;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System.Linq;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;

    public interface IDailyProblemClassificationAgencyService
    {
        IDailyProblemClassificationAgencyService Start();

        IDailyProblemClassificationAgencyService GetPADailyAgencyScanFromQueue();

        IEnumerable<ProblemAccountLosingAgencyEntity> GetProblemAccountLosingClassification(FunctionId funcId);

        IProblemWLClassificationAgencyService GetInstanceProblemWLClassificationAgencyService();

        bool UpdateComplete();
    }

    public class DailyProblemClassificationAgencyService : IDailyProblemClassificationAgencyService
    {
        private readonly IProblemClassificationAgencyDataAccess _dataAccess;
        private readonly IProblemAccountAgencyService _problemAccountAgencyService;
        private readonly IServiceProvider _serviceProvider;

        private IEnumerable<long> _paDailyScanFromQueue;


        public DailyProblemClassificationAgencyService(
            IProblemClassificationAgencyDataAccess dataAccess,
            IProblemAccountAgencyService problemAccountAgencyService,
            IServiceProvider serviceProvider)
        {
            _dataAccess = dataAccess;
            _problemAccountAgencyService = problemAccountAgencyService;
            _serviceProvider = serviceProvider;
        }

        public IDailyProblemClassificationAgencyService Start()
        {
            _paDailyScanFromQueue = new List<long>();

            return this;
        }

        public IDailyProblemClassificationAgencyService GetPADailyAgencyScanFromQueue()
        {
            _paDailyScanFromQueue = _dataAccess.GetPADailyScanFromQueue();

            return this;
        }

        public IEnumerable<ProblemAccountLosingAgencyEntity> GetProblemAccountLosingClassification(FunctionId funcId)
        {
            if (_paDailyScanFromQueue.IsNotNullOrEmpty())
            {
                return _problemAccountAgencyService.GetProblemAccountLosingAgencies(_paDailyScanFromQueue);
            }

            return Enumerable.Empty<ProblemAccountLosingAgencyEntity>();
        }

        public bool UpdateComplete()
        {
            string custIdToString = string.Empty;

            if (_paDailyScanFromQueue.IsNotNullOrEmpty())
            {
                custIdToString = string.Join(",", _paDailyScanFromQueue.Distinct());
            }

            _dataAccess.CompletePADailyScan(custIdToString);

            return _paDailyScanFromQueue.IsNotNullOrEmpty();
        }

        public IProblemWLClassificationAgencyService GetInstanceProblemWLClassificationAgencyService()
        {
            return _serviceProvider.GetService<IProblemWLClassificationAgencyService>();
        }
    }
}
