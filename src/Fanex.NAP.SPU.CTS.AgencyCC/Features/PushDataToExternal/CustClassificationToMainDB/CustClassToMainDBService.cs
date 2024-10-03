namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Sentry;

    public interface ICustClassToMainDBService
    {
        #region Agency General
        bool PushExternalPackageByCustId(IEnumerable<long> custIds, PushDataToExternalModel paras);

        public bool PushCustomerClassification(IEnumerable<LatestClassificationModel> customerClassifications, int retryTime, string serviceName);
        #endregion
    }

    public class CustClassToMainDBService : ICustClassToMainDBService
    {
        private readonly ICustClassToMainDBDataAccess _dataAccess;
        private readonly ISentryClient _sentryClient;

        public CustClassToMainDBService(ICustClassToMainDBDataAccess dataAccess, ISentryClient sentryClient)
        {
            _dataAccess = dataAccess;
            _sentryClient = sentryClient;
        }

        #region Agency General
        public bool PushExternalPackageByCustId(IEnumerable<long> custIds, PushDataToExternalModel parameters)
        {
            if (custIds.IsNullOrEmpty())
            {
                return true;
            }

            var custIdsChunk = custIds.Distinct().Chunk(parameters.ExternalBatchSize);

            foreach (var custids in custIdsChunk)
            {
                var lastestClassifications = GetLatestCCAndDangerLevel(custids);

                if (lastestClassifications != null && lastestClassifications.Any())
                {
                    bool isSuccess = IsPushExternalSuccess(lastestClassifications, parameters);

                    if (!isSuccess)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool PushCustomerClassification(IEnumerable<LatestClassificationModel> customerClassifications, int retryTime, string serviceName)
        {
            if (customerClassifications == null || !customerClassifications.Any())
            {
                return true;
            }

            while (retryTime >= 0)
            {
                try
                {
                    var result = _dataAccess.UpdateCustomerClassification(customerClassifications);

                    if (result.ErrorCode != 0 && retryTime == 0)
                    {
                        Utilities.LogSentryError(serviceName, "PushCustomerClassificationToMain fail", customerClassifications, new Exception(result.ErrorMsg), _sentryClient);
                        return false;
                    }
                    else if (result.ErrorCode != 0)
                    {
                        retryTime--;
                    }
                    else
                    {
                        retryTime = -1;
                    }
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
                isSuccess = PushCustClassificationToMainDB(lastestClassifications, paras.ServiceName);

                //if (paras.IsUpdateDangerToMainDB && isSuccess)
                //{
                //    isSuccess = UpdateDangerLevelToMainDB(lastestClassifications, paras.ServiceName);
                //}
            }

            return isSuccess;
        }

        private bool PushCustClassificationToMainDB(IEnumerable<LatestCCDangerLevelEntity> lastestClassifications, string serviceName)
        {
            if (lastestClassifications != null && lastestClassifications.Any())
            {
                var custClassification = lastestClassifications.Select(c => new LatestClassificationModel()
                {
                    CustId = c.CustID,
                    Category = c.CategoryID,
                    CustomerClass = c.CustomerClass
                });

                return PushCustomerClassification(custClassification, Constants.RetryPushCCToExternal, serviceName);
            }

            return true;
        }
        #endregion
    }
}
