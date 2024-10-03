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

        public int ActiveDays { get; set; }

        public DateTime PerformanceTime { get; set; }
    }
}