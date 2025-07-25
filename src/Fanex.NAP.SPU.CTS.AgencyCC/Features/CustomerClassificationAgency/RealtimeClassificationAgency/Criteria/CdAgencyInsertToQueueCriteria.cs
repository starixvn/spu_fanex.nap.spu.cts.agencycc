namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency
{
    using Fanex.Data;
    using Fanex.Data.Repository;   

    public class CdAgencyInsertToQueueCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_CustIDs")]
        public string CustIDList { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_ConsiderableDanger_Queue_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
