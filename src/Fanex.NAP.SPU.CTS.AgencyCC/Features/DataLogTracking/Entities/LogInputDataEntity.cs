namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.DataLogTracking.Entities
{
    using Newtonsoft.Json;

    public class LogInputDataEntity
    {
        [JsonProperty("CustID")]
        public long? CustId { get; set; }

        [JsonProperty("InputValue")]
        public object InputValue { get; set; }
    }
}
