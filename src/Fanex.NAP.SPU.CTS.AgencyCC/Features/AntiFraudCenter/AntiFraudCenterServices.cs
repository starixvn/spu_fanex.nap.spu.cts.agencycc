namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.AntiFraudCenter
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface IAntiFraudCenterServices
    {
        void InsertAFCAccounts(IEnumerable<long> custIds, FromFlow fromFlow, bool isSyncAFCData, string url);

        void DeleteAFCAccounts(IEnumerable<long> custIds, FromFlow fromFlow, bool isSyncAFCData, string url);
    }

    public class AntiFraudCenterServices : IAntiFraudCenterServices
    {
        private readonly ISentryClient _sentryClient;
        private const string ServiceName = "AntiFraudCenter";

        public AntiFraudCenterServices(ISentryClient sentryClient)
        {
            _sentryClient = sentryClient;
        }

        #region Public methods

        public async void InsertAFCAccounts(IEnumerable<long> custIds, FromFlow fromFlow, bool isSyncAFCData, string url)
        {
            try
            {
                if (custIds.IsNotNullOrEmpty() && isSyncAFCData)
                {
                    await CallAPI(custIds, fromFlow, url);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(ServiceName, $" {fromFlow} - CTS.CC Service - Insert Accounts fail", custIds, ex, _sentryClient, "CustIds");
            }
        }

        public async void DeleteAFCAccounts(IEnumerable<long> custIds, FromFlow fromFlow, bool isSyncAFCData, string url)
        {
            try
            {
                if (custIds.IsNotNullOrEmpty() && isSyncAFCData)
                {
                    await CallAPI(custIds, fromFlow, url);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(ServiceName, $" {fromFlow} - CTS.CC Service - Delete Accounts fail", custIds, ex, _sentryClient, "CustIds");
            }
        }

        #endregion Public methods

        #region Private methods

        private static async Task<bool> CallAPI(IEnumerable<long> custIds, FromFlow fromFlow, string url)
        {
            if (custIds.IsNotNullOrEmpty())
            {
                var para = new
                {
                    CustIds = custIds,
                    FromFlow = fromFlow
                };

                return await ApiHelper.Post<bool>(url, para, 300);
            }

            return true;
        }

        #endregion Private methods
    }
}
