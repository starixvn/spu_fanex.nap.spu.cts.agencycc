namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency.Criteria
{
    using Fanex.Data.Repository;

    public class DailyNormalClassificationAgencyPreprocessCriteria : NonQueryCommand
    {
        public string ListCustId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_GeneralNormalClassificationAgency_Daily_Preprocess";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}