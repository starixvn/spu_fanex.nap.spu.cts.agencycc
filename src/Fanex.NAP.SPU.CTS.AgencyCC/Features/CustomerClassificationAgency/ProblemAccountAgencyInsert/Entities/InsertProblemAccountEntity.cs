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
            //TVSReasonID = data.TVSReasonID;
            //TVSRequestID = data.TVSRequestID;
            //IsParlay = data.IsParlay;
            SportType = data.SportType;
            //RobotCounter = data.RobotCounter;
            CategoryID = data.CategoryID;
            RoleID = data.RoleID;
            SubscriberID = data.SubscriberID;
            IsLicensee = data.IsLicensee;
            Remark = data.Remark;
            CreatedBy = data.CreatedBy;
            IsMarkedDirectly = data.IsMarkedDirectly;
            //IsFromTVS = data.IsFromTVS;
            //IsFromTW = data.IsFromTW;
            IsFromCTS = data.IsFromCTS;
            //IsFromAI = data.IsFromAI;
            //IsFromImperva = data.IsFromImperva;
            LastModifiedDate = data.LastModifiedDate;
            CreatedDate = data.CreatedDate;
        }

        public Int64 CTSCustID { get; set; }

        public int? RoleID { get; set; }

        public int SubscriberID { get; set; }

        //public int? TVSReasonID { get; set; }

        public bool IsLicensee { get; set; }

        public Int64? CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public string Remark { get; set; }

        public bool? IsMarkedDirectly { get; set; }

        public int? WinlossStatus { get; set; }

        public bool IsFromCTS { get; set; }

        //public bool IsFromTVS { get; set; }

        //public bool IsFromTW { get; set; }

        //public bool IsFromAI { get; set; }

        //public bool IsFromImperva { get; set; }

        //public int? TVSRequestID { get; set; }

        //public bool? IsParlay { get; set; }

        public int? SportType { get; set; }

        public string SportName { get; set; }

        public int? IssueTypeID { get; set; }

        //public int? MatchID { get; set; }

        //public int? RobotCounter { get; set; }

        public string GroupName { get; set; }

        public int? GBTicketCount { get; set; }

        public DateTime? DetectedDate { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
