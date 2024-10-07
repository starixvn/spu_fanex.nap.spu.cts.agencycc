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

        public decimal? LastXDaysTurnoverRM { get; set; }

        public decimal? LastXDaysWinlossRM { get; set; }

        public Int64? LastXDaysBetCount { get; set; }

        public decimal? LastYDaysTurnoverRM { get; set; }

        public decimal? LastYDaysWinlossRM { get; set; }

        public Int64? LastYDaysBetCount { get; set; }

        public DateTime? PerformanceTime { get; set; }
    }
}
