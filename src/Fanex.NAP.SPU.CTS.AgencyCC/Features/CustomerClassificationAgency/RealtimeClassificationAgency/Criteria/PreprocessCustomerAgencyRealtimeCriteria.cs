namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Criteria
{
    using Fanex.Data.Repository;

    public class PreprocessCustomerAgencyRealtimeCriteria : NonQueryCommand
    {
        public string ListCustId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_GeneralNormalClassificationAgency_Realtime_Preprocess"; // new
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
