namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    public class GetChangeCustomerAgencyRealtimeCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ScannedMaxId")]
        public long ScannedMaxId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_GeneralNormalClassificationAgency_RealTime_GetChanges"; // new
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
