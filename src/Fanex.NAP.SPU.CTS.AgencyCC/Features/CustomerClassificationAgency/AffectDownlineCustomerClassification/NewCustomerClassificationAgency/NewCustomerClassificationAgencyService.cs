using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter;
using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
using System.Linq;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency
{
    public interface INewCustomerClassificationAgencyService
    {
        INewCustomerClassificationAgencyService Start();

        INewCustomerClassificationAgencyService GetCategoryForNewCTSCustomerAgency();

        INewCustomerClassificationAgencyService InsertVVIPForNewCustomerAgency(RescanParametersModel parameters);

        INewCustomerClassificationAgencyService InsertPAForNewCustomerAgency(RescanParametersModel parameters);

        INewCustomerClassificationAgencyService RescanNormalForNewCustomerAgency(RescanParametersModel parameters);

        bool UpdateComplete();
    }

    public class NewCustomerClassificationAgencyService : INewCustomerClassificationAgencyService
    {
        private readonly INewCustomerClassificationAgencyDataAccess _dataAccess;
        private readonly IProblemAccountAgencyService _problemAccountAgencyService;
        private readonly ICustClassToMainDBService _custClassToMainDBService;
        private readonly IRescanCustClassificationAgencyService _rescanCustClassificationAgencyService;
        private readonly ISystemParameterServices _systemParameterServices;
        private NewCustomerClassificationAgencyModel _newCustomerClassification;
        private bool _isSuccess;

        public NewCustomerClassificationAgencyService(
            INewCustomerClassificationAgencyDataAccess dataAccess,
            IProblemAccountAgencyService problemAccountAgencyService,
            ICustClassToMainDBService custClassToMainDBService,
            IRescanCustClassificationAgencyService rescanCustClassificationAgencyService, 
            ISystemParameterServices systemParameterServices)
        {
            _dataAccess = dataAccess;
            _problemAccountAgencyService = problemAccountAgencyService;
            _custClassToMainDBService = custClassToMainDBService;
            _rescanCustClassificationAgencyService = rescanCustClassificationAgencyService;
            _systemParameterServices = systemParameterServices;
        }

        public INewCustomerClassificationAgencyService Start()
        {
            _newCustomerClassification = new NewCustomerClassificationAgencyModel();
            _isSuccess = true;

            return this;
        }

        public INewCustomerClassificationAgencyService GetCategoryForNewCTSCustomerAgency()
        {
            _newCustomerClassification = _dataAccess.GetNewCTSCustomerCategoryAgency();

            return this;
        }

        public INewCustomerClassificationAgencyService InsertVVIPForNewCustomerAgency(RescanParametersModel parameters)
        {
            if (_isSuccess && _newCustomerClassification.VVIPClassification.IsNotNullOrEmpty())
            {
                // affectDownline.InsertVVIP
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

        public INewCustomerClassificationAgencyService RescanNormalForNewCustomerAgency(RescanParametersModel parameters)
        {
            if (_isSuccess && _newCustomerClassification.NormalClassification.IsNotNullOrEmpty())
            {
                var custIDs = _newCustomerClassification.NormalClassification.Select(x => x.CustID);

                _isSuccess = _rescanCustClassificationAgencyService.RescanCustomerClassificationAgency(custIDs, parameters);
            }

            return this;
        }

        public bool UpdateComplete()
        {
            if (_isSuccess)
            {
                _isSuccess = _systemParameterServices.UpdateTimingExecution(_newCustomerClassification.LastCTSCustID.ToString(), CTS_SystemParameters.ReScanNewMemberAgency_LastCTSCustID);
            }

            return _isSuccess;
        }
    }
}
