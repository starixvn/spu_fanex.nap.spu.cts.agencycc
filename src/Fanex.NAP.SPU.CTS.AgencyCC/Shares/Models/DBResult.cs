namespace Fanex.NAP.SPU.CTS.AgencyCC.Shares.Models
{
    using System.Collections;
    using System.Text.Json.Serialization;
    
    public class DBResult
    {
        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        [JsonIgnore]
        public Hashtable OutParam { get; set; }
    }
}
