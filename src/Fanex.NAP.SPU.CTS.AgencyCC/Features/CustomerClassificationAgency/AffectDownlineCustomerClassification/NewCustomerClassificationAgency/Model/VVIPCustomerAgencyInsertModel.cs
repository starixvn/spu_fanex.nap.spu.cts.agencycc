namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Model
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Entities;
    using System;
    using System.Collections.Generic;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public class VVIPCustomerAgencyInsertModel
    {
        public FromFlow FromFlow { get; set; }

        public int? UplineRoleID { get; set; }

        public Int64? QueueID { get; set; }

        public bool? IsAffectDownline { get; set; }

        public IEnumerable<VvipCategoryAgencyEntity> CustInfo { get; set; }
    }
}
