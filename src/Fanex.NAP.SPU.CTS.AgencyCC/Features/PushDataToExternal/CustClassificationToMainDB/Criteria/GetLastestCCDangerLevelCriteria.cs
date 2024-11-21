namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetLastestCCDangerLevelCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_CustIDs")]
        public string CustIds { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassification_GetCCAndDangerLevel";
        }

        public override bool IsValid()
            => true;
    }
}