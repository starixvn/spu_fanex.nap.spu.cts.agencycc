namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetCustomerCategoryAgencyRealtimeCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_CustIDs")]
        public string CustIdList { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_Realtime_GetCategory";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(CustIdList);
        }
    }
}
