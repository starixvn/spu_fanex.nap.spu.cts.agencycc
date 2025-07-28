namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{
    public class ConsiderableDangerRecheckEntity
    {
        public long CustID { get; set; }
        
        public long CTSCustID { get; set; }

        public int CategoryID { get; set; }

        public int RoleID { get; set; }

        public int SubscriberID { get; set; }

        public bool IsLicensee { get; set; }

    }
}
