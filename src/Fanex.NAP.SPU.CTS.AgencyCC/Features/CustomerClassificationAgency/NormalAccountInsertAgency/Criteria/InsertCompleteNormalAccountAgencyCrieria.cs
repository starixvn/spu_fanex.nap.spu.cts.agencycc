namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class InsertCompleteNormalAccountAgencyCrieria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ScannedMaxId")]
        public long ScannedMaxId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_GeneralNormalClassificationAgency_NormalAccount_Complete";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
