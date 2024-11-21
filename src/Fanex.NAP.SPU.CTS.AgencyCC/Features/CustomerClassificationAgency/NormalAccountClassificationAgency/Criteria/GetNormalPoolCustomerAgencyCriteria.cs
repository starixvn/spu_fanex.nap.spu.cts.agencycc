namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    class GetNormalPoolCustomerAgencyCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ScannedMaxId")]
        public long ScannedMaxId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_GeneralNormalClassificationAgency_NormalPool_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
