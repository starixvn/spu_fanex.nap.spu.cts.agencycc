namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class NormalClassificationAgencyRescanCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ListCustId")]
        public string ListCustId { get; set; }

        [SpParam(SqlParamName = "FunctionId")]
        public int FunctionId { get; set; }

        [SpParam(SqlParamName = "RescanTypeId")]
        public int RescanTypeId { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_GeneralNormalClassificationAgency_RescanNormal_Preprocess";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}