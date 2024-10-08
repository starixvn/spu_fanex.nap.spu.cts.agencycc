namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public class RescanParametersModel
    {
        public RescanParametersModel()
        {
            IsWriteLog = false;
        }

        public bool IsToQueue { get; set; }

        public FunctionId FunctionId { get; set; }

        public RescanNormalType RescanNormalType { get; set; }

        public int InternalBatchSize { get; set; }

        public string ServiceName { get; set; }

        public bool IsWriteLog { get; set; }

        public PushDataToExternalModel PushDataExternal { get; set; }
    }
}
