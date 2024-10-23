namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.AntiFraudCenter;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Sentry;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface ICustClassToMainDBService
    {
        #region Agency General
        bool PushExternalPackageByCustId(IEnumerable<long> custIds, PushDataToExternalModel paras);

        bool PushExternalProblemAcountPackage(IEnumerable<long> custIds, PushDataToExternalModel paras);

        public bool PushCustomerClassification(IEnumerable<LatestClassificationModel> customerClassifications, RoleGroupType roleGroup, int retryTime, string serviceName);
        #endregion
    }

    public class CustClassToMainDBService : ICustClassToMainDBService
    {
        private readonly ICustClassToMainDBDataAccess _dataAccess;
        private readonly ISentryClient _sentryClient;
        private readonly IAntiFraudCenterServices _antiFraudCenterServices;

        public CustClassToMainDBService(ICustClassToMainDBDataAccess dataAccess, ISentryClient sentryClient, IAntiFraudCenterServices antiFraudCenterServices)
        {
            _dataAccess = dataAccess;
            _sentryClient = sentryClient;
            _antiFraudCenterServices = antiFraudCenterServices;
        }

        #region Agency General
        public bool PushExternalPackageByCustId(IEnumerable<long> custIds, PushDataToExternalModel paras)
        {
            if (custIds.IsNullOrEmpty())
            {
                return true;
            }

            var custIdsChunk = custIds.Distinct().Chunk(paras.ExternalBatchSize);

            foreach (var custids in custIdsChunk)
            {
                var lastestClassifications = GetLatestCCAndDangerLevel(custids);

                if (lastestClassifications != null && lastestClassifications.Any())
                {
                    bool isSuccess = IsPushExternalSuccess(lastestClassifications, paras);

                    if (!isSuccess)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool PushCustomerClassification(IEnumerable<LatestClassificationModel> customerClassifications, RoleGroupType roleGroup, int retryTime, string serviceName)
        {
            if (customerClassifications.IsNullOrEmpty())
            {
                return true;
            }

            while (retryTime >= 0)
            {
                try
                {
                    var result = _dataAccess.UpdateCustomerClassification(customerClassifications, roleGroup);

                    if (result.ErrorCode == 200)
                    {
                        Utilities.LogSentryError(
                            serviceName, $"PushCustomerClassificationToMain throw exception: not existing CustIDs in mainDB",
                            customerClassifications, new Exception(result.ErrorMsg), _sentryClient);
                    }

                    if (retryTime == 0)
                    {
                        Utilities.LogSentryError(
                            serviceName, $"PushCustomerClassificationToMain fail with error code: {result.ErrorCode}",
                            customerClassifications, new Exception(result.ErrorMsg), _sentryClient);
                    }

                    if (result.ErrorCode == 200 || result.ErrorCode == 0)
                    {
                        return true;
                    }

                    if (retryTime == 0)
                    {
                        return false;
                    }

                    retryTime--;
                }
                catch (Exception ex)
                {
                    if (retryTime == 0)
                    {
                        Utilities.LogSentryError(serviceName, "PushCustomerClassificationToMain throw exception", customerClassifications, ex, _sentryClient);
                        return false;
                    }

                    retryTime--;
                }
            }

            return true;
        }

        public bool PushExternalProblemAcountPackage(IEnumerable<long> custIds, PushDataToExternalModel paras)
        {
            var isSuccess = true;

            if (custIds != null && custIds.Any())
            {
                isSuccess = PushExternalPackageByCustId(custIds.Distinct(), paras);

                _antiFraudCenterServices.InsertAFCAccounts(custIds.Distinct(), FromFlow.CTSServices, paras.IsSyncAFCData, paras.AFCInsertAccountURL);
            }

            return isSuccess;
        }
        #endregion

        #region private method
        private IEnumerable<LatestCCDangerLevelEntity> GetLatestCCAndDangerLevel(IEnumerable<long> custIds)
        {
            if (custIds != null && custIds.Any())
            {
                return _dataAccess.GetLatestCCAndDangerLevel(custIds.Distinct());
            }

            return Enumerable.Empty<LatestCCDangerLevelEntity>();
        }

        private bool IsPushExternalSuccess(IEnumerable<LatestCCDangerLevelEntity> lastestClassifications, PushDataToExternalModel paras)
        {
            bool isSuccess = true;

            if (lastestClassifications != null && lastestClassifications.Any())
            {
                isSuccess = PushCustClassificationToMainDB(lastestClassifications, paras.RoleGroup, paras.ServiceName);

#pragma warning disable S125 // Sections of code should not be commented out
                //if (paras.IsUpdateDangerToMainDB && isSuccess)
                //{
                //    isSuccess = UpdateDangerLevelToMainDB(lastestClassifications, paras.ServiceName);
                //}
#pragma warning restore S125 // Sections of code should not be commented out
            }

            return isSuccess;
        }

        private bool PushCustClassificationToMainDB(IEnumerable<LatestCCDangerLevelEntity> lastestClassifications, RoleGroupType roleGroup, string serviceName)
        {
            if (lastestClassifications != null && lastestClassifications.Any())
            {
                var custClassification = lastestClassifications.Select(c => new LatestClassificationModel
                {
                    CustId = c.CustID,
                    Category = c.CategoryID,
                    CustomerClass = c.CustomerClass
                });

                return PushCustomerClassification(custClassification, roleGroup, Constants.RetryPushCCToExternal, serviceName);
            }

            return true;
        }
        #endregion
    }
}
