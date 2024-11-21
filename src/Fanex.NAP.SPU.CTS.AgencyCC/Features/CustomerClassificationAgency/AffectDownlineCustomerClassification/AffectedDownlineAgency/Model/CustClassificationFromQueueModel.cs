namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
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
    }
}
