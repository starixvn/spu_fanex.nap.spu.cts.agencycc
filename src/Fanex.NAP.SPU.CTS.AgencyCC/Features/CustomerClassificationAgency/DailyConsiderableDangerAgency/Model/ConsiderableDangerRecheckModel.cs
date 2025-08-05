namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency
{
    using System.Collections.Generic;
    using System.Linq;

    public class ConsiderableDangerRecheckModel
    {
        public ConsiderableDangerRecheckModel()
        {
            RecheckRatioCusts = Enumerable.Empty<ConsiderableDangerRecheckEntity>();

            RecheckWLCusts = Enumerable.Empty<ConsiderableDangerRecheckEntity>();
        }

        public long LastID { get; set; }

        public IEnumerable<ConsiderableDangerRecheckEntity> RecheckRatioCusts { get; set; }

        public IEnumerable<ConsiderableDangerRecheckEntity> RecheckWLCusts { get; set; }
    }
}
