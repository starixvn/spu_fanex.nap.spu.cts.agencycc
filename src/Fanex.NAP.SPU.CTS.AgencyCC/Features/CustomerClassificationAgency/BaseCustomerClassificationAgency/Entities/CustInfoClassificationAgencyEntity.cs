namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities
{
    using System;

    public class CustInfoClassificationAgencyEntity
    {
        public long CTSCustID { get; set; }

        public long CustID { get; set; }

        public Int16 RoleID { get; set; }

        public int CustomerClass { get; set; }

        public int CategoryID { get; set; }

        public DateTime ScannedTime { get; set; }

        public bool IsPA { get; set; }
    }
}
