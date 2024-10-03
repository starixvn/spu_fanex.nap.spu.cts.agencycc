namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities
{
    using System;

    public class CustomerClassificationAgencyEntity
    {
        public long CustID { get; set; }

        public int? CategoryID { get; set; }

        public int? CategoryGroupID { get; set; }

        public decimal? TurnoverRM { get; set; }

        public decimal? WinlossRM { get; set; }

        public Int64? BetCount { get; set; }

        public int? ActiveDays { get; set; }

        public DateTime? PerformanceTime { get; set; }
    }
}
