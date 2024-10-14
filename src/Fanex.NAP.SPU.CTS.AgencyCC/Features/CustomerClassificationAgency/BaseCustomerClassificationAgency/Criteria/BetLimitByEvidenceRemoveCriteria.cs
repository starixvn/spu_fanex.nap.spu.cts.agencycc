namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class BetLimitByEvidenceRemoveCriteria : NonQueryCommand
    {
        public string CustIDList { get; set; }

        [SpParam(SqlParamName = "op_ErrorMessage")]
        public string ErrorMessage { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BLControl_EvidenceUnflagged";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
