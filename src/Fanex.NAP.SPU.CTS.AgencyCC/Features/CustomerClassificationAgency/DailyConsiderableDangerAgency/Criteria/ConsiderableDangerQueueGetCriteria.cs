namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ConsiderableDangerQueueGetCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_BatchSize")]
        public int BatchSize { get; set; }

        [SpParam(SqlParamName = "op_LastID")]
        public long LastID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_Considerable_Queue_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
