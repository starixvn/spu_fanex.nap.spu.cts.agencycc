namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency.Criteria
{
    using Fanex.Data.Repository;

    public class PADailyScanFromQueueAgencyCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_DailyScanPA_GetFromQueue";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
