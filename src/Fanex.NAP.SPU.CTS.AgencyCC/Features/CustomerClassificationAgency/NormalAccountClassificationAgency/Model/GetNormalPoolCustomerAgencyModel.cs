namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Model
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Entities;
    using System.Collections.Generic;

    public class GetNormalPoolCustomerAgencyModel
    {
        public GetNormalPoolCustomerAgencyModel()
        {
            this.NormalPoolCustomers = new List<GetNormalPoolCustomerAgencyEntity>();
            this.ScannedMaxId = 0;
        }

        public IEnumerable<GetNormalPoolCustomerAgencyEntity> NormalPoolCustomers { get; set; }

        public long ScannedMaxId { get; set; }
    }
}
