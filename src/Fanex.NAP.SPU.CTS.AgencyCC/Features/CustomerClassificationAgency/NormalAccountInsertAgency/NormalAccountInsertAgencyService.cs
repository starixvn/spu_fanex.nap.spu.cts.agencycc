namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Newtonsoft.Json;
    using Sentry;
    using System.Collections.Generic;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface INormalAccountInsertAgencyService
    {
        GetNormalAccountAgencyModel GetNormalAccounts();

        ResultInsertCustomerClassificationAgencyModel InsertNormalAccount(IEnumerable<InsertNormalAccountAgencyEntity> custs, string serviceName, bool isWriteLog);

        void CompleteNormalAccountInsert(long scannedMaxId, bool isSuccess);
    }

    public class NormalAccountInsertAgencyService : BaseCustomerClassificationAgencyService, INormalAccountInsertAgencyService
    {
        private readonly INormalAccountInsertAgencyDataAccess _dataAccess;
        public NormalAccountInsertAgencyService(
            IBaseCustomerClassificationAgencyDataAccess baseCCAgencyDataAccess,
            ISentryClient sentryClient,
            INormalAccountInsertAgencyDataAccess dataAccess) : base(baseCCAgencyDataAccess, sentryClient)
        {
            _dataAccess = dataAccess;
        }

        public GetNormalAccountAgencyModel GetNormalAccounts()
        {
            return _dataAccess.GetNormalAccount();
        }

        public ResultInsertCustomerClassificationAgencyModel InsertNormalAccount(IEnumerable<InsertNormalAccountAgencyEntity> custs, string serviceName, bool isWriteLog)
        {
            var result = new ResultInsertCustomerClassificationAgencyModel();

            if (custs.IsNotNullOrEmpty())
            {
                var modelInsert = new InsertCustomerClassificationAgencyModel
                {
                    FromAction = (int)FromFlow.CTSServices,
                    CustInfoJson = JsonConvert.SerializeObject(custs)
                };

                result = base.InsertCustomerClassification(AgencyCCInputFlow.Normal, modelInsert, serviceName);
            }

            return result;
        }

        public void CompleteNormalAccountInsert(long scannedMaxId, bool isSuccess)
        {
            if (isSuccess)
            {
                _dataAccess.CompleteNormalAccountInsert(scannedMaxId);
            }
        }
    }
}
