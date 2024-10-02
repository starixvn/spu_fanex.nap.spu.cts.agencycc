namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    class DailyNormalScanningAgencyGetCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "op_LastCategoryID")]
        public int LastCategoryID { get; set; }

        [SpParam(SqlParamName = "op_LastCustID")]
        public UInt64 LastCustID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_DailyScan_GetNormal"; // new
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
