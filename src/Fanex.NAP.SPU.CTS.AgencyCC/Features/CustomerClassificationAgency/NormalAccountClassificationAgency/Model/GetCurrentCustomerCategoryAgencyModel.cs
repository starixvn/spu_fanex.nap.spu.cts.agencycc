namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Model
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;

    [XName("r")]
    public class GetCurrentCustomerCategoryAgencyModel
    {
        [XName("CustId")]
        public long CustId { get; set; }

        [XName("CategoryId")]
        public int CategoryId { get; set; }

        [XName("IsNewCreated")]
        public bool IsNewCreated { get; set; }
    }
}
