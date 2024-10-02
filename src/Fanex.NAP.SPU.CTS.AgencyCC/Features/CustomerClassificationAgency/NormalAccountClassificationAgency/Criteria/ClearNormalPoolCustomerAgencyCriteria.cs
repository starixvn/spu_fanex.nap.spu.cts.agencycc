namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Criteria
{
    using Fanex.Data.Repository;

    public class ClearNormalPoolCustomerAgencyCriteria : NonQueryCommand
    {
        public string ListCustId { get; set; }

        public long ScannedMaxId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_GeneralNormalClassificationAgency_NormalPool_Clear";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
