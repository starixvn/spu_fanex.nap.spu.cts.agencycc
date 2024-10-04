using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter;
using Sentry;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CTSCustomerAgency
{
    public interface ICTSCustomerAgencyJobService
    {
        Task<bool> InactivateNewCustomers(int batchSize);
    }

    public class CTSCustomerAgencyJobService : ICTSCustomerAgencyJobService
    {
        private readonly ISentryClient _sentryClient;
        private readonly ICTSCustomerAgencyService _service;
        private readonly ISystemParameterServices _systemParameter;
        private readonly IRescanCustClassificationAgencyService _rescanCustClassification;
        private readonly IAppSettings _appSettings;

        public CTSCustomerAgencyJobService(
            ISentryClient sentryClient,
            ICTSCustomerAgencyService service,
            ISystemParameterServices systemParameter,
            IRescanCustClassificationAgencyService rescanCustClassification,
            IAppSettings appSettings)
        {
            _sentryClient = sentryClient;
            _service = service;
            _systemParameter = systemParameter;
            _rescanCustClassification = rescanCustClassification;
            _appSettings = appSettings;
        }

        public async Task<bool> InactivateNewCustomers(int batchSize)
        {
            try
            {
                var isContinue = true;

                do
                {
                    var inactivateCusts = _service.GetInactivateNewCustomers();

                    if (inactivateCusts.CustInfos != null && inactivateCusts.CustInfos.Any())
                    {
                        var custIDs = inactivateCusts.CustInfos.Select(x => x.CustID).Distinct();

                        var rescanConfig = new RescanParametersModel()
                        {
                            IsToQueue = false,
                            FunctionId = FunctionIdAgency.InactivateNewCustomer,
                            RescanNormalType = RescanNormalType.Auto_Customer,
                            InternalBatchSize = batchSize,
                            ServiceName = "InactiveNewCustomerAgency",
                            PushDataExternal = new PushDataToExternalModel
                            {
                                IsUpdateDangerToMainDB = false, // not update for agency
                                ExternalBatchSize = batchSize,
                                ServiceName = "InactiveNewCustomerAgency",
                                IsSyncAFCData = _appSettings.IsSyncAFCData,
                                AFCInsertAccountURL = _appSettings.AFCInsertAccountURL,
                                AFCDeleteAccountURL = _appSettings.AFCDeleteAccountURL
                            }
                        };

                        _rescanCustClassification.RescanCustomerClassificationAgency(custIDs, rescanConfig);

                        isContinue = true;
                    }
                    else
                    {
                        isContinue = false;
                    }

                    _systemParameter.UpdateTimingExecution(inactivateCusts.LastCategoryID.ToString(), CTS_SystemParameters.InactiveNewAgency_LastCategoryID);
                    _systemParameter.UpdateTimingExecution(inactivateCusts.LastCustID.ToString(), CTS_SystemParameters.InactiveNewAgency_LastCustID);

                    Thread.Sleep(10000);
                } while (isContinue);

                return true;
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                      .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                      .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                      .ToString();

                _sentryClient.CaptureMessage(logMessage, Sentry.Protocol.SentryLevel.Error);
                return false;
            }
        }
    }
}
