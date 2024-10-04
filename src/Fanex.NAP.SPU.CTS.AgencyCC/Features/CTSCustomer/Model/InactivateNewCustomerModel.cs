namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer.Model
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;

    public class InactivateNewCustomerModel
    {
        public IEnumerable<RescanCategoryDailyModel> CustInfos { get; set; }

        public UInt64 LastCustID { get; set; }

        public int LastCategoryID { get; set; }
    }
}
