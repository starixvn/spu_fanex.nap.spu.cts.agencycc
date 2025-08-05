namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{
    using Fanex.Data;
    using Fanex.Data.Repository;   
   
    public class ConsiderableDangerDetectCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_Cust")]
        public string CustIDs { get; set; }

        [SpParam(SqlParamName = "ip_IsRescan")]
        public bool IsRescan { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_DetectCD";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
