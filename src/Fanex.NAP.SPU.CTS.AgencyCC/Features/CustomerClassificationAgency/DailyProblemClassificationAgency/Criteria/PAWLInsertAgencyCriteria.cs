namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class PAWLInsertAgencyCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_ProblemAccount")]
        public string ProblemAccountJson { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_DailyScanPA_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
