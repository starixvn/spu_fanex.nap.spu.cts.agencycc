namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.ArchiveData.ClassificationHistory
{
    using Fanex.Data;
    using Fanex.Data.Repository;
   
    public class HistoryClassificationArchiveCompleteCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_MaxArchiveID")]
        public long MaxArchiveID { get; set; }

        [SpParam(SqlParamName = "ip_ListArchiveID")]
        public string ListArchiveID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_History_Archive_Complete";
        }

        public override bool IsValid()
            => true;
    }
}