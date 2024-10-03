namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Sentry;
    using System;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IBaseCustomerClassificationAgencyService
    {
        ResultInsertCustomerClassificationAgencyModel InsertCustomerClassification(AgencyCCInputFlow inputFlow, InsertCustomerClassificationAgencyModel model, string serviceName);
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
    }
}
