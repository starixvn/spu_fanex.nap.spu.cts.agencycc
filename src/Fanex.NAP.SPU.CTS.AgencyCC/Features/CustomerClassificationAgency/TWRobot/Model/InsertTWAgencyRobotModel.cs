namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.TWRobot
{
    using System.Collections.Generic;
    using System.Linq;

    public class InsertTWAgencyRobotModel
    {
        public InsertTWAgencyRobotModel()
        {
            Robots = Enumerable.Empty<TWRobotUserEntity>();
        }

        public IEnumerable<TWRobotUserEntity> Robots { get; set; }
    }
}
