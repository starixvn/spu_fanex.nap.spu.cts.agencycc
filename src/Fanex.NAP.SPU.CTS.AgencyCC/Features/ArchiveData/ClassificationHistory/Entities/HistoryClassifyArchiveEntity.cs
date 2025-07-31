namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.ArchiveData.ClassificationHistory
{
    using System;


    public class HistoryClassifyArchiveEntity
    {
        public long ID { get; set; }

        public long CustID { get; set; }

        public long CTSCustID { get; set; }

        public int RoleID { get; set; }

        public int ParentID { get; set; }

        public int CategoryID { get; set; }

        public int OldCategoryID { get; set; }

        public int DWCategoryID { get; set; }

        public int TargetCC { get; set; }

        public int SourceTypeID { get; set; }

        public bool IsDataChanged { get; set; }

        public int ActionType { get; set; }

        public bool IsAuto { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime InsertDate { get; set; }

        public int TargetDangerLevel1 { get; set; }

        public string Remark { get; set; }

        public bool IsMarkedDirectly { get; set; }

        public bool IsFromTW { get; set; }

        public bool IsFromCTS { get; set; }

        public bool IsFromAI { get; set; }

        public decimal TurnoverRM { get; set; }

        public decimal WinlossRM { get; set; }

        public long BetCount { get; set; }

        public decimal LastXDaysTurnoverRM { get; set; }

        public decimal LastXDaysWinlossRM { get; set; }

        public long LastXDaysBetCount { get; set; }

        public decimal LastYDaysTurnoverRM { get; set; }

        public decimal LastYDaysWinlossRM { get; set; }

        public long LastYDaysBetCount { get; set; }

        public DateTime? PerformanceTime { get; set; }

        public int RobotCounter { get; set; }

        public int CustomerCategorySettingsAgency { get; set; }

        public string SystemRemark { get; set; }

    }
}
