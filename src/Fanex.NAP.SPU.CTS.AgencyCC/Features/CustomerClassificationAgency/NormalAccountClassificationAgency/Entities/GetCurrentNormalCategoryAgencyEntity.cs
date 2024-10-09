namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Entities
{
    using System;

    public class GetCurrentNormalCategoryAgencyEntity
    {
        public long CustID { get; set; }

        public Int16 RoleID { get; set; }

        public long CategoryID { get; set; }

        public bool IsNewCreated { get; set; }
    }
}
