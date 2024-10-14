namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
    using System.Collections.Generic;
    using System.Linq;

    public class CustClassificationFromQueueModel
    {
        public CustClassificationFromQueueModel()
        {
            this.AgencyQueue = Enumerable.Empty<CustClassificationFromQueueEntity>();            
        }

        public IEnumerable<CustClassificationFromQueueEntity> AgencyQueue { get; set; }

        public int ActionType { get; set; }

        public int UplineRoleID { get; set; }

        public long QueueID { get; set; }

        public long LastDownlineCTSCustID { get; set; }     

        public RescanParametersModel RescanParameters { get; set; }
    }
}
