namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Model
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Entities;
    using System.Collections.Generic;

    public class GetNormalAccountAgencyModel
    {
        public GetNormalAccountAgencyModel()
        {
            this.NormalAccount = new List<GetNormalAccountAgencyEntity>();
            this.IsContinute = false;
        }

        public IEnumerable<GetNormalAccountAgencyEntity> NormalAccount { get; set; }

        public long ScannedMaxId { get; set; }

        public bool IsContinute { get; set; }
    }
}
