namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class RoleCustomerGetCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_CustIDs")]
        public string CustIds { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustInfo_GetCustIDsRole";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}