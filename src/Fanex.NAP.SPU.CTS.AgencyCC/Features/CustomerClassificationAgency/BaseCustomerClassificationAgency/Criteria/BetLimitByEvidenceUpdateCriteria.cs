namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class BetLimitByEvidenceUpdateCriteria : NonQueryCommand
    {
        public string CustIDList { get; set; }

        [SpParam(SqlParamName = "op_ErrorMessage")]
        public string ErrorMessage { get; set; }

        public override string GetSettingKey()
            => "NAP_BLControl_EvidenceFlagged";

        public override bool IsValid()
            => true;
    }
}
