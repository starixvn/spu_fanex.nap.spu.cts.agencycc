namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetRescanCustomerClassificationAgencyCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_CustIDs")]
        public string CustIDs { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_Rescan_Get"; // new
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
