namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Model
{
    using System.Collections.Generic;

    public class GetCustomerCategoryAgencyRealtimeModel
    {
        public GetCustomerCategoryAgencyRealtimeModel()
        {
            CustomerCategory = new List<long>();
            CustomerPADailyScan = new List<long>();
        }

        public List<long> CustomerCategory { get; set; }

        public List<long> CustomerPADailyScan { get; set; }
    }
}
