namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.ArchiveData.ClassificationHistory
{
    using Fanex.Data;
    using Fanex.Data.Repository;    

    public class HistoryClassificationArchiveGetCriteria : CriteriaBase
    {       
        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassificationAgency_History_Archive_Get";
        }

        public override bool IsValid()
            => true;
    }
}
