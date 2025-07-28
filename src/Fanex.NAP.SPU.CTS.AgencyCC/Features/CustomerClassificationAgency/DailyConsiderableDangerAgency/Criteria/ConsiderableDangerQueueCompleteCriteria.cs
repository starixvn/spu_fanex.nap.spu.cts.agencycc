namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{
    using Fanex.Data;
    using Fanex.Data.Repository;   

    public class ConsiderableDangerQueueCompleteCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_LastID")]
        public long LastID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_ConsiderableDanger_Queue_Complete";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
