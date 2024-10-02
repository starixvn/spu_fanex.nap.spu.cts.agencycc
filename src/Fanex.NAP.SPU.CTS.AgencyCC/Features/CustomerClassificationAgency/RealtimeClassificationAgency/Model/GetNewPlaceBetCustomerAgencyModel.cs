namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Model
{
    using System.Collections.Generic;

    public class GetNewPlaceBetCustomerAgencyModel
    {
        public IEnumerable<long> CustIds { get; set; }

        public long ScannedMaxId { get; set; }
    }
}
