namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Entities
{
    using System;

    public class GetNormalAccountAgencyEntity
    {
        public long CustId { get; set; }

        public int? CategoryId { get; set; }

        public int? CategoryGroupId { get; set; }

        public DateTime CreatedTime { get; set; }

        public decimal TurnoverRM { get; set; }

        public decimal WinlossRM { get; set; }

        public Int64 BetCount { get; set; }

        public decimal? LastXDaysTurnoverRM { get; set; }

        public decimal? LastXDaysWinlossRM { get; set; }

        public Int64? LastXDaysBetCount { get; set; }

        public decimal? LastYDaysTurnoverRM { get; set; }

        public decimal? LastYDaysWinlossRM { get; set; }

        public Int64? LastYDaysBetCount { get; set; }

        public DateTime PerformanceTime { get; set; }
    }
}