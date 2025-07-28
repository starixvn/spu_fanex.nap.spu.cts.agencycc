namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{
    using Fanex.Data;
    using Fanex.Data.Repository;   
   
    public class ConsiderableDangerDetectCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_CustIDs")]
        public string CustIDs { get; set; } 

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_DetectConsiderableDanger";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
