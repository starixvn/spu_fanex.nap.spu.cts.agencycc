namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Model
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System;

    [XName("r")]
    public class GetCurrentCustomerCategoryAgencyModel
    {
        [XName("CustId")]
        public long CustId { get; set; }

        [XName("CategoryId")]
        public long CategoryId { get; set; }

        [XName("RoleId")]
        public Int16 RoleId { get; set; }

        [XName("IsNewCreated")]
        public bool IsNewCreated { get; set; }

        [XName("IsRealtimeOnly")]
        public bool IsRealtimeOnly { get; set; }
    }
}
