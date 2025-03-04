namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert
{
    using Newtonsoft.Json;
    using System;

    public class ProblemAccountLosingAgencyEntity
    {      
        [JsonProperty("CustID")]
        public long CustId { get; set; }

        public int WinlossStatus { get; set; }

        public decimal TurnoverRM { get; set; }

        public decimal WinlossRM { get; set; }

        public long BetCount { get; set; }

        public int ActiveDays { get; set; }

        public DateTime? PerformanceTime { get; set; }
    }
}
