namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;
    using System;  

    public class InsertProblemAccountEntity : CustomerClassificationAgencyEntity
    {
        public InsertProblemAccountEntity() { }

        public InsertProblemAccountEntity(RescanCustomerClassificationAgencyEntity data)
        {
            CTSCustID = data.CTSCustID;
            CustID = data.CustID;                     
            CategoryID = data.CategoryID;
            RoleID = data.RoleID;
            SubscriberID = data.SubscriberID;
            IsLicensee = data.IsLicensee;
            Remark = data.Remark;
            CreatedBy = data.CreatedBy;
            IsMarkedDirectly = data.IsMarkedDirectly;           
            IsFromCTS = data.IsFromCTS;                  
            CreatedDate = data.CreatedDate;
        }

        public Int64 CTSCustID { get; set; }

        public int? RoleID { get; set; }

        public int SubscriberID { get; set; }
       
        public bool IsLicensee { get; set; }

        public Int64? CreatedBy { get; set; }
        
        public string Remark { get; set; }

        public bool? IsMarkedDirectly { get; set; }

        public int? WinlossStatus { get; set; }

        public bool IsFromCTS { get; set; }

        public bool IsFromTW { get; set; }

        public bool IsFromAI { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
