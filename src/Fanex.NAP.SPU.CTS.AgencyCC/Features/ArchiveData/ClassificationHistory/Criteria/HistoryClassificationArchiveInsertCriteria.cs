namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.ArchiveData.ClassificationHistory
{
    using Fanex.Data;
    using Fanex.Data.Repository;  

    public class HistoryClassificationArchiveInsertCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_HistoryJson")]
        public string HistoryDataJson { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_Archive_CustClassificationAgency_History_Insert";
        }

        public override bool IsValid()
            => true;
    }
}
