namespace Fanex.NAP.SPU.CTS.AgencyCC.Shares
{
    using System.ComponentModel;

    public static class Constants
    {
        public const int RetryPushCCToExternal = 3;

        public enum CTS_SystemParameters
        {
            DailyScan_Agency_Normal_LastCategoryID = 176,
            DailyScan_Agency_Normal_LastCustID = 177,
            InactiveNewAgency_LastCategoryID = 178,
            InactiveNewAgency_LastCustID = 179
        };

        public enum Main_SystemParameters
        {
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
            [Description("VVIP")]
            VVIP = 1,
            [Description("SpecialCC")]
            SpecialCC = 2,
            [Description("LicVIP")]
            LicVIP = 3,
            [Description("LicBA")]
            LicBA = 4,
            [Description("InsertPACategory")]
            InsertPACategory = 5,
            [Description("InsertPAReason")]
            InsertPAReason = 6,
            [Description("Potential")]
            Potential = 8,
            [Description("Normal")]
            Normal = 9,
            [Description("RescanPACategory")]
            RescanPACategory = 115,
            [Description("RescanPotential")]
            RescanPotential = 118,
            [Description("RescanNormal")]
            RescanNormal = 9,
            [Description("RemoveVVIP")]
            RemoveVVIP = 221,
            [Description("RemoveSpecialCC")]
            RemoveSpecialCC = 222,
            [Description("RemoveLicVIP")]
            RemoveLicVIP = 223,
            [Description("RemoveLicBA")]
            RemoveLicBA = 224,
            [Description("RemovePACategory")]
            RemovePACategory = 225,
            [Description("BySportInsertSpecialCC")]
            BySportInsertSpecialCC = 332,
            [Description("BySportInsertNormal")]
            BySportInsertNormal = 339,
            [Description("BySportRemoveSpecialCC")]
            BySportRemoveSpecialCC = 442
        }

        public enum FunctionIdAgency
        {
            InactivateNewCustomer = 11,
        }

        public enum RescanNormalType
        {
            Manual = 4,
            Auto_Customer = 5,
            Auto_AfftectedDownline = 6
        }

        public enum CCInputFlowAgency
        {
            [Description("VVIP")]
            VVIP = 1,
            [Description("InsertPACategory")]
            InsertPACategory = 5,
            [Description("RescanNormal")]
            RescanNormal = 9,
            [Description("RemoveVVIP")]
            RemoveVVIP = 221,
            [Description("RemovePACategory")]
            RemovePACategory = 225,
        }

        public enum CustomerRole
        {
            Member = 1,
            Agent = 2,
            Master = 3,
            Super = 4
        };
    }
}
