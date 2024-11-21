namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    using System;
    using System.Collections.Generic;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public class VvipCustomerInsertEntity
    {
        public FromFlow FromFlow { get; set; }

        public int? UplineRoleID { get; set; }

        public Int64? QueueID { get; set; }

        public bool? IsAffectDownline { get; set; }

        public IEnumerable<VvipCategoryEntity> CustInfo { get; set; }
    }
}
