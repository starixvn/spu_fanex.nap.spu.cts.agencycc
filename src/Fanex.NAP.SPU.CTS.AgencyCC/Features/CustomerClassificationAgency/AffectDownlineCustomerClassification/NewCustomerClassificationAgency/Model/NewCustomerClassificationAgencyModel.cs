namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Entitites;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert.Model;

    public class NewCustomerClassificationAgencyModel
    {
        public NewCustomerClassificationAgencyModel()
        {
            VVIPClassification = Enumerable.Empty<VVIPCategoryAgencyEntity>();
            PAClassification = Enumerable.Empty<InsertProblemAccountAgencyModel>();
            NormalClassification = Enumerable.Empty<NormalRescanAgencyEntity>();
        }

        public UInt64 LastCTSCustID { get; set; }

        public IEnumerable<VVIPCategoryAgencyEntity> VVIPClassification { get; set; }

        public IEnumerable<InsertProblemAccountAgencyModel> PAClassification { get; set; }

        public IEnumerable<NormalRescanAgencyEntity> NormalClassification { get; set; }
    }
}
