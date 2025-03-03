#pragma warning disable S2342
namespace Fanex.NAP.SPU.CTS.AgencyCC.Shares
{
    using System.ComponentModel;

    public static class Constants
    {
        public const int RetryPushCCToExternal = 3;
#pragma warning disable S2148 // Underscores should be used to make large numbers readable
        public const int StarixITId = 10278938;
#pragma warning restore S2148 // Underscores should be used to make large numbers readable

        public const string CATEGORYTYPE_AGENCY_CATEID_ROBOT = "CONST_AGENCY_CATEID_ROBOT";

        public enum CTS_SystemParameters
        {
            DailyScan_Agency_Normal_LastCategoryID = 176,
            DailyScan_Agency_Normal_LastCustID = 177,
            InactiveNewAgency_LastCategoryID = 179,
            InactiveNewAgency_LastCustID = 180,
        };

        public enum Main_SystemParameters
        {
        }

        public enum DownlineClassification_ActionType
        {
            InsertVVIP = 1,
            RemoveVVIP = 2,
            InsertPA = 3,
            RemovePA = 4,
        }

        public enum FromFlow
        {
            CTSWEB = 1,
            CTSAPI = 2,
            CTSServices = 3,
            ManualTrigger = 4
        };

        public enum AgencyCCInputFlow
        {
            [Description("InsertVVIP")]
            InsertVVIP = 1001,
            [Description("InsertPACategory")]
            InsertPACategory = 1005,
            [Description("InsertPAReason")]
            InsertPAReason = 1006,
            [Description("Normal")]
            Normal = 1009,
            //[Description("RescanPACategory")]
            //RescanPACategory = 1115,
            [Description("RemovePACategory")]
            RemovePACategory = 1225,
            [Description("RescanNormal")]
            RescanNormal = 1009,
            [Description("RemoveVVIP")]
            RemoveVVIP = 1221,
        }

        public enum FunctionId
        {
            InsertNewCustJob = 4,
            InactivateNewCustomer = 11,            
            RobotUserJob = 23,
            DownlineCustomerClassification = 52,
            ProblemDailyJob = 9999
        }

        public enum RescanNormalType
        {
            Auto_Customer = 5,
            Auto_AfftectedDownline = 6,
            Auto_InactiveCustomer = 8
        }

        public enum CustomerRole
        {
            Member = 1,
            Agent = 2,
            Master = 3,
            Super = 4
        };

        public enum RoleGroupType
        {
            Member = 1,
            Agency = 2
        }
    }
}
#pragma warning restore S2342
