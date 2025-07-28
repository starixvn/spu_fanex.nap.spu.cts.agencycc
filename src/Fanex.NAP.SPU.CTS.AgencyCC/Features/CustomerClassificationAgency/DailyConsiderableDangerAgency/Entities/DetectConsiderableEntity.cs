namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{    
    public class DetectConsiderableEntity
    {
        public long CustID { get; set; }

        public bool IsPassRuleConsiderable { get; set; }

        public bool IsPassRuleRatio { get; set; }
    }
}
