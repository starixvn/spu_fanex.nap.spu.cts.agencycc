namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;

    [XName("r")]
    public class LatestClassificationModel
    {
        [XName("id")]
        public long CustId { get; set; }

        [XName("c")]
        public int CustomerClass { get; set; }

        [XName("t")]
        public int Category { get; set; }
    }
}
