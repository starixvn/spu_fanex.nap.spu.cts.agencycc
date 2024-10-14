using Fanex.NAP.SPU.CTS.AgencyCC.Features.AntiFraudCenter;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter;
using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
using Newtonsoft.Json;
using Sentry;
using System.Linq;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency
{
    public interface INewCustomerClassificationAgencyService
    {
        INewCustomerClassificationAgencyService Start();

        INewCustomerClassificationAgencyService InitialData(NewCustomerClassificationAgencyModel model);

        INewCustomerClassificationAgencyService InsertVVIPForNewCustomerAgency(RescanParametersModel parameters);

        INewCustomerClassificationAgencyService InsertPAForNewCustomerAgency(RescanParametersModel parameters);

        bool RescanNormalForNewCustomerAgency(RescanParametersModel parameters);
    }

    public class NewCustomerClassificationAgencyService : INewCustomerClassificationAgencyService
    {
        private readonly IProblemAccountAgencyService _problemAccountAgencyService;
        private readonly ICustClassToMainDBService _custClassToMainDBService;
        private readonly IRescanCustClassificationAgencyService _rescanCustClassificationAgencyService;
        private readonly IAntiFraudCenterServices _antiFraudCenterServices;
        private readonly IBaseCustomerClassificationAgencyService _baseCustomerClassificationAgencyService;
        private NewCustomerClassificationAgencyModel _newCustomerClassification;
        private bool _isSuccess;

        public NewCustomerClassificationAgencyService(
            IBaseCustomerClassificationAgencyService baseCustomerClassificationAgencyService,
            IProblemAccountAgencyService problemAccountAgencyService,
            ICustClassToMainDBService custClassToMainDBService,
            IRescanCustClassificationAgencyService rescanCustClassificationAgencyService,
            IAntiFraudCenterServices antiFraudCenterServices)
        {
            _problemAccountAgencyService = problemAccountAgencyService;
            _custClassToMainDBService = custClassToMainDBService;
            _rescanCustClassificationAgencyService = rescanCustClassificationAgencyService;
            _antiFraudCenterServices = antiFraudCenterServices;
            _baseCustomerClassificationAgencyService = baseCustomerClassificationAgencyService;
        }

        public INewCustomerClassificationAgencyService Start()
        {
            _newCustomerClassification = new NewCustomerClassificationAgencyModel();
            _isSuccess = true;

            return this;
        }

        public INewCustomerClassificationAgencyService InitialData(NewCustomerClassificationAgencyModel model)
        {
            _newCustomerClassification = model;

            return this;
        }

        public INewCustomerClassificationAgencyService InsertVVIPForNewCustomerAgency(RescanParametersModel parameters)
        {
            if (_isSuccess && _newCustomerClassification.VVIPClassification.IsNotNullOrEmpty())
            {
                var custInfo = new VVIPCustomerAgencyInsertModel
                {
                    CustInfo = _newCustomerClassification.VVIPClassification,
                    FromFlow = FromFlow.CTSServices,
                    IsAffectDownline = false,
                    QueueID = null,
                    UplineRoleID = null
                };

                _isSuccess = InsertVVIP(parameters, custInfo);
            }

            return this;
        }

        public INewCustomerClassificationAgencyService InsertPAForNewCustomerAgency(RescanParametersModel parameters)
        {
            if (_isSuccess && _newCustomerClassification.PAClassification.IsNotNullOrEmpty())
            {
                var result = _problemAccountAgencyService.InsertProblemAccountsAgencyPackage(_newCustomerClassification.PAClassification,
                    FunctionId.InsertNewCustJob,
                    AgencyCCInputFlow.InsertPACategory,
                    parameters.IsToQueue, parameters.ServiceName);

                _isSuccess = _custClassToMainDBService.PushExternalProblemAcountPackage(result, parameters.PushDataExternal);
            }

            return this;
        }

        public bool RescanNormalForNewCustomerAgency(RescanParametersModel parameters)
        {
            if (_isSuccess && _newCustomerClassification.NormalClassification.IsNotNullOrEmpty())
            {
                var custIDs = _newCustomerClassification.NormalClassification.Select(x => x.CustID);

                _isSuccess = _rescanCustClassificationAgencyService.RescanCustomerClassificationAgency(custIDs, parameters);
            }

            return _isSuccess;
        }

        #region private method
        private bool InsertVVIP(RescanParametersModel parameters, VVIPCustomerAgencyInsertModel custInfo)
        {
            var isSuccess = true;

            if (custInfo != null && custInfo.CustInfo.Any())
            {
                var modelInsert = new InsertCustomerClassificationAgencyModel()
                {
                    CustInfoJson = JsonConvert.SerializeObject(custInfo.CustInfo),
                    FromAction = (int)custInfo.FromFlow,
                    UplineRoleID = custInfo.UplineRoleID,
                    QueueID = custInfo.QueueID,
                    IsAffectDownline = custInfo.IsAffectDownline,
                    IsToQueue = parameters.IsToQueue
                };

                var result = _baseCustomerClassificationAgencyService.InsertCustomerClassification(AgencyCCInputFlow.InsertVVIP, modelInsert, parameters.ServiceName);

                isSuccess = string.IsNullOrEmpty(result.ErrorMessage);

                if (result.CustInfos != null && result.CustInfos.Any())
                {
                    var custIDs = result.CustInfos.Select(c => c.CustID).Distinct();

                    isSuccess = _custClassToMainDBService.PushExternalPackageByCustId(custIDs, new PushDataToExternalModel
                    {
                        RoleGroup = RoleGroupType.Agency,
                        ExternalBatchSize = parameters.PushDataExternal.ExternalBatchSize,
                        IsUpdateDangerToMainDB = false,
                        IsSyncAFCData = false,
                        ServiceName = parameters.ServiceName
                    });

                    _antiFraudCenterServices.DeleteAFCAccounts(custIDs, FromFlow.CTSServices, parameters.PushDataExternal.IsSyncAFCData, parameters.PushDataExternal.AFCDeleteAccountURL);
                }
            }

            return isSuccess;
        }
        #endregion
    }
}
