namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model
{
    using System;

    public class InsertCustomerClassificationAgencyModel
    {
        public string CustInfoJson { get; set; }

        public int FromAction { get; set; }

        public int? UplineRoleID { get; set; }

        public Int64? QueueID { get; set; }

        public bool? IsAffectDownline { get; set; }

        public bool IsToQueue { get; set; }
    }
}
