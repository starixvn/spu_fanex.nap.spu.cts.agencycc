namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Model
{
    using System.Collections.Generic;

    public class GetCustomerCategoryAgencyRealtimeModel
    {
        public GetCustomerCategoryAgencyRealtimeModel()
        {
            CustomerCategory = new List<ulong>();
            CustomerPADailyScan = new List<ulong>();
            CustomerConsiderableDanger = new List<ulong>(); 
        }

        public List<ulong> CustomerCategory { get; set; }

        public List<ulong> CustomerPADailyScan { get; set; }

        public List<ulong> CustomerConsiderableDanger { get; set; }

        public void AddItems(GetCustomerCategoryAgencyRealtimeModel custCates)
        {
            CustomerCategory.AddRange(custCates.CustomerCategory);
            CustomerPADailyScan.AddRange(custCates.CustomerPADailyScan);
            CustomerConsiderableDanger.AddRange(custCates.CustomerConsiderableDanger);
        }
    }
}
