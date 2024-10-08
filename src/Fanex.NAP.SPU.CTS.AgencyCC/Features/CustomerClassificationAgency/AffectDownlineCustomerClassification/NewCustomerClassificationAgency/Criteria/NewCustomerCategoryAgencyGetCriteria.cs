namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Criteria
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class NewCustomerCategoryAgencyGetCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "op_LastCTSCustID")]
        public UInt64 LastCTSCustID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustInfo_CheckCategoryInfoNewCustAgency"; // new
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}