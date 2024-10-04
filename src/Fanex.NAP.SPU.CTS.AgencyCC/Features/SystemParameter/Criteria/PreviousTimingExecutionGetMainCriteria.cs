namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter.Criteria
{
    using Fanex.Data.Repository;

    public class PreviousTimingExecutionGetMainCriteria : CriteriaBase
    {
        public int ParamID { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_SystemParameter_Get";
        }

        public override bool IsValid()
            => true;
    }
}