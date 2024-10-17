namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Entitites;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;

    public class NewCustomerClassificationAgencyModel
    {
        public NewCustomerClassificationAgencyModel()
        {
            VVIPClassification = Enumerable.Empty<VvipCategoryAgencyEntity>();
            PAClassification = Enumerable.Empty<InsertProblemAccountEntity>();
            NormalClassification = Enumerable.Empty<NormalRescanAgencyEntity>();
        }

        public UInt64 LastCTSCustID { get; set; }

        public IEnumerable<VvipCategoryAgencyEntity> VVIPClassification { get; set; }

        public IEnumerable<InsertProblemAccountEntity> PAClassification { get; set; }

        public IEnumerable<NormalRescanAgencyEntity> NormalClassification { get; set; }
    }
}
