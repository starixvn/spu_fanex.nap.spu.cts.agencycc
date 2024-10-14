namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
    using System.Collections.Generic;
    using System.Linq;

    public class CustClassificationFromQueueModel
    {
        public CustClassificationFromQueueModel()
        {
            this.AgencyQueue = Enumerable.Empty<CustClassificationFromQueueEntity>();
            this.PushDataExternal = new PushDataToExternalEntity();
        }

        public IEnumerable<CustClassificationFromQueueEntity> AgencyQueue { get; set; }

        public int ActionType { get; set; }

        public int UplineRoleID { get; set; }

        public long QueueID { get; set; }

        public long LastDownlineCTSCustID { get; set; }

        public bool IsToQueue { get; set; }

        public PushDataToExternalEntity PushDataExternal { get; set; }
    }
}
