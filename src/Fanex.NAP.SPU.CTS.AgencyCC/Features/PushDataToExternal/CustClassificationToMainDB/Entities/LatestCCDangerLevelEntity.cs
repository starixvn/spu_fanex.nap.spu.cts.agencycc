namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Entities
{
    public class LatestCCDangerLevelEntity
    {
        public long CustID { get; set; }

        public int CategoryID { get; set; }

        public int CustomerClass { get; set; }

        public int? DangerLevel { get; set; }

        public int? DangerLevelType { get; set; }
    }
}
