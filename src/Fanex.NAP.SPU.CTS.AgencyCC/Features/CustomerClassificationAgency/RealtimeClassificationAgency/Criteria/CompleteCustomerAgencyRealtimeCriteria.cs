namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Criteria
{
    using Fanex.Data.Repository;

    public class CompleteCustomerAgencyRealtimeCriteria : NonQueryCommand
    {
        public long ScannedMaxId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_GeneralNormalClassificationAgency_RealTime_Complete";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
