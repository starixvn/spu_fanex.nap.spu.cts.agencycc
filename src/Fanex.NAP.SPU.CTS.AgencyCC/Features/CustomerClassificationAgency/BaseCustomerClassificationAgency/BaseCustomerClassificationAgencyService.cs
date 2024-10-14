namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares.Models;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IBaseCustomerClassificationAgencyService
    {
        ResultInsertCustomerClassificationAgencyModel InsertCustomerClassification(AgencyCCInputFlow inputFlow, InsertCustomerClassificationAgencyModel model, string serviceName);

        GetRescanCustomerClassificationAgencyModel GetRescanCustomerClassificationAgency(string custIDs);

        DBResult UpdateBetLimitOfFlaggedCustomer(string custIDs);

        DBResult UpdateBetLimitOfUnflaggedCustomer(string custIDs);
    }

    public class BaseCustomerClassificationAgencyService : IBaseCustomerClassificationAgencyService
    {
        private readonly IBaseCustomerClassificationAgencyDataAccess _dataAccess;
        private readonly ISentryClient _sentryClient;

        public BaseCustomerClassificationAgencyService(IBaseCustomerClassificationAgencyDataAccess dataAccess, ISentryClient sentryClient)
        {
            _dataAccess = dataAccess;
            _sentryClient = sentryClient;
        }

        public ResultInsertCustomerClassificationAgencyModel InsertCustomerClassification(AgencyCCInputFlow inputFlow, InsertCustomerClassificationAgencyModel model, string serviceName)
        {
            var result = _dataAccess.InsertCustomerClassification(inputFlow, model);

            if (result != null && result.ErrorMessage.IsNotNullOrEmpty())
            {
                Utilities.LogSentryError(serviceName, "InsertCustomerClassificationAgency throw Exception", model, new Exception(result.ErrorMessage), _sentryClient);
            }

            return result;
        }

        public GetRescanCustomerClassificationAgencyModel GetRescanCustomerClassificationAgency(string custIDs)
        {
            return _dataAccess.GetRescanCustomerClassificationAgency(custIDs);
        }

        //public IEnumerable<ProblemAccountLosingClassificationEntity> GetProblemAccountLosingClassification(IEnumerable<long> custIds, FunctionId funcId)
        //{
        //    if (custIds != null && custIds.Any())
        //    {
        //        var custIdsToString = string.Join(",", custIds);

        //        return problemAccountDataAccess.GetProblemAccountLosingClassification(custIdsToString, funcId);
        //    }

        //    return Enumerable.Empty<ProblemAccountLosingClassificationEntity>();
        //}

        public DBResult UpdateBetLimitOfFlaggedCustomer(string custIDs)
        {
            return _dataAccess.UpdateBetLimitOfFlaggedCustomer(custIDs);
        }

        public DBResult UpdateBetLimitOfUnflaggedCustomer(string custIDs)
        {
            return _dataAccess.UpdateBetLimitOfUnflaggedCustomer(custIDs);
        }
    }
}
