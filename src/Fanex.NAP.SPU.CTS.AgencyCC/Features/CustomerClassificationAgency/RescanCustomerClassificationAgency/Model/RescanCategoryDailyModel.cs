namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;

    [XName("r")]
    public class RescanCategoryDailyModel
    {
        [XName("CustId")]
        public long CustID { get; set; }

        [XName("CategoryId")]
        public int CurrentCategoryID { get; set; }
    }
}
