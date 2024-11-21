namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class ClassifyNormalPoolCustomerAgencyCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "CustomersXML")]
        public string CustomersXML { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_GeneralNormalClassificationAgency_NormalPool_Classify";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
