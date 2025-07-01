namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares.Models;
    using System;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IBaseCustomerClassificationAgencyService
    {
        ResultInsertCustomerClassificationAgencyModel InsertCustomerClassification(AgencyCCInputFlow inputFlow, InsertCustomerClassificationAgencyModel model, string serviceName);

        GetRescanCustomerClassificationAgencyModel GetRescanCustomerClassificationAgency(string custIDs);

        ResultRemoveCustomerClassificationAgencyModel RemoveCustomerClassificationAgency(AgencyCCInputFlow inputFlow, RemoveCustomerClassificationAgencyModel model);

        DBResult UpdateBetLimitOfFlaggedCustomer(string custIDs);

        DBResult UpdateBetLimitOfUnflaggedCustomer(string custIDs);

        int? GetCategoryByCategoryTypeName(string categoryTypeName);
    }

    public class BaseCustomerClassificationAgencyService : IBaseCustomerClassificationAgencyService
    {
        private readonly IBaseCustomerClassificationAgencyDataAccess _dataAccess;

        public BaseCustomerClassificationAgencyService(IBaseCustomerClassificationAgencyDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public ResultInsertCustomerClassificationAgencyModel InsertCustomerClassification(AgencyCCInputFlow inputFlow, InsertCustomerClassificationAgencyModel model, string serviceName)
        {
            var result = _dataAccess.InsertCustomerClassification(inputFlow, model);

            if (result != null && result.ErrorMessage.IsNotNullOrEmpty())
            {
                Utilities.LogSentryError(serviceName, "InsertCustomerClassificationAgency throw Exception", model, new Exception(result.ErrorMessage));
            }

            return result;
        }

        public GetRescanCustomerClassificationAgencyModel GetRescanCustomerClassificationAgency(string custIDs)
        {
            return _dataAccess.GetRescanCustomerClassificationAgency(custIDs);
        }

        public ResultRemoveCustomerClassificationAgencyModel RemoveCustomerClassificationAgency(AgencyCCInputFlow inputFlow, RemoveCustomerClassificationAgencyModel model)
        {
            return _dataAccess.RemoveCustomerClassificationAgency(inputFlow, model);
        }

#pragma warning disable S125
        //public IEnumerable<ProblemAccountLosingClassificationEntity> GetProblemAccountLosingClassification(IEnumerable<long> custIds, FunctionId funcId)
        //{
        //    if (custIds != null && custIds.Any())
        //    {
        //        var custIdsToString = string.Join(",", custIds);

        //        return problemAccountDataAccess.GetProblemAccountLosingClassification(custIdsToString, funcId);
        //    }

        //    return Enumerable.Empty<ProblemAccountLosingClassificationEntity>();
        //}
#pragma warning restore S125

        public DBResult UpdateBetLimitOfFlaggedCustomer(string custIDs)
        {
            return _dataAccess.UpdateBetLimitOfFlaggedCustomer(custIDs);
        }

        public DBResult UpdateBetLimitOfUnflaggedCustomer(string custIDs)
        {
            return _dataAccess.UpdateBetLimitOfUnflaggedCustomer(custIDs);
        }

        public int? GetCategoryByCategoryTypeName(string categoryTypeName)
        {
            return _dataAccess.GetCategoryByCategoryTypeName(categoryTypeName);
        }
    }
}
