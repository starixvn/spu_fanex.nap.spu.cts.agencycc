namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model
{
    public class PushDataToExternalModel
    {
        public PushDataToExternalModel()
        {
            IsWriteLog = false;
            IsUpdateDangerToMainDB = true;
        }

        public bool IsUpdateDangerToMainDB { get; set; }

        public int ExternalBatchSize { get; set; }

        public string ServiceName { get; set; }

        public bool IsWriteLog { get; set; }

        //AFC
        public bool IsSyncAFCData { get; set; }

        public string AFCInsertAccountURL { get; set; }

        public string AFCDeleteAccountURL { get; set; }

        public bool IsSMACustomer { get; set; }
    }
}
