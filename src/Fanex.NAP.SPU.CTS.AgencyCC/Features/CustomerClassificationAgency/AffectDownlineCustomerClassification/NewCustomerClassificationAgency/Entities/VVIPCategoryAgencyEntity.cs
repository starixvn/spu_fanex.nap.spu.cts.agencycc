namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Entities
{
    public class VvipCategoryAgencyEntity
    {
        public long CTSCustID { get; set; }

        public long CustID { get; set; }

        public int RoleID { get; set; }

        public int SubscriberID { get; set; }

        public int? CategoryID { get; set; }

        public int CreatedBy { get; set; }

        public string Remark { get; set; }
    }
}
