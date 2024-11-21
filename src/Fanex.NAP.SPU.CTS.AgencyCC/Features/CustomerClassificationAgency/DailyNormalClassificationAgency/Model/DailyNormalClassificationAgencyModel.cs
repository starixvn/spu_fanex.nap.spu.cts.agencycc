namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency.Model
{
    using System;
    using System.Collections.Generic;

    public class DailyNormalClassificationAgencyModel
    {
        public int LastCategoryID { get; set; }

        public UInt64 LastCustID { get; set; }

        public IEnumerable<Int64> CustID { get; set; }
    }
}
