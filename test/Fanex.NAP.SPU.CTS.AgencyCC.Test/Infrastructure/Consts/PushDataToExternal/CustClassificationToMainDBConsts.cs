namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Consts.PushDataToExternal
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3898:Value types should implement \"IEquatable<T>\"", Justification = "<Pending>")]
    public struct CustClassificationToMainDBConsts
    {
        public struct PrivateMethodName
        {
            public const string PushCustClassificationToMainDB = "PushCustClassificationToMainDB";

            public const string IsPushExternalSuccess = "IsPushExternalSuccess";

            public const string GetLatestCCAndDangerLevel = "GetLatestCCAndDangerLevel";

            public const string UpdateDangerLevelToMainDB = "UpdateDangerLevelToMainDB";

            public const string PushDangerLevel = "PushDangerLevel";

            public const string GetCustomerClassificationBySports = "GetCustomerClassificationBySports";
        }
    }
}
