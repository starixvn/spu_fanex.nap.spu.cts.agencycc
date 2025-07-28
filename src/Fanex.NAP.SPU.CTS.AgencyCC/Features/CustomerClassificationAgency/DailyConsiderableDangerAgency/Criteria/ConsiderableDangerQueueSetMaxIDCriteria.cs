namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{
    using Fanex.Data.Repository;
  
    public class ConsiderableDangerQueueSetMaxIDCriteria : NonQueryCommand
    {
        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_Considerable_Queue_MaxID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
