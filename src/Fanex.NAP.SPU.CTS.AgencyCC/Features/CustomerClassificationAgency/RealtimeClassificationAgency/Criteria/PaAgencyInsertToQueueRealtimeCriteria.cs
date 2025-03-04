namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency
{
    using Fanex.Data;
    using Fanex.Data.Repository;   

    public class PaAgencyInsertToQueueRealtimeCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_CustIDs")]
        public string CustIDList { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_DailyScanPA_InsertToQueue";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
