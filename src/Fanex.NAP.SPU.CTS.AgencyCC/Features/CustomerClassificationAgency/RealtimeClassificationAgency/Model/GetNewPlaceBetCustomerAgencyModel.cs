namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Model
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Entities;
    using System.Collections.Generic;

    public class GetNewPlaceBetCustomerAgencyModel
    {
        public IEnumerable<GetNewPlaceBetCustomerAgencyEntity> CustomerNewPlaceBets { get; set; }

        public long ScannedMaxId { get; set; }
    }
}
