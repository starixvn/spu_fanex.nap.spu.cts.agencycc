namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class PADailyScanCompleteAgencyCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_CustIDs")]
        public string CustIDs { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_DailyScanPA_Complete";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
