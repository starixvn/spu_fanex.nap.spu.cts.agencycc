namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetNormalAccountAgencyCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ScannedMaxId")]
        public long ScannedMaxId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_GeneralNormalClassificationAgency_NormalAccount_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}