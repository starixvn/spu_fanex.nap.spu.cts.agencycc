namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Model
{
    using System.Collections.Generic;

    public class GetNormalPoolCustomerAgencyModel
    {
        public GetNormalPoolCustomerAgencyModel()
        {
            this.NormalPoolCustomers = new List<long>();
            this.ScannedMaxId = 0;
        }

        public IEnumerable<long> NormalPoolCustomers { get; set; }

        public long ScannedMaxId { get; set; }
    }
}
