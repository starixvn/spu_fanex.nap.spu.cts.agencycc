namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter
{
    using System;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface ISystemParameterServices
    {
        Int64 GetLastScannedId(CTS_SystemParameters paraID);

        UInt64 GetLastScannedId2(CTS_SystemParameters paraID);

        DateTime? GetLastScannedDateTime(CTS_SystemParameters paraID);

        bool UpdateTimingExecution(string paraValue, CTS_SystemParameters paramId);
    }

    public class SystemParameterServices : ISystemParameterServices
    {
        private readonly ISystemParameterDataAccess systemParameterDataAccess;

        public SystemParameterServices(ISystemParameterDataAccess systemParameterDataAccess)
        {
            this.systemParameterDataAccess = systemParameterDataAccess;
        }

#pragma warning disable S2589 // Boolean expressions should not be gratuitous
#pragma warning disable S6580
        public Int64 GetLastScannedId(CTS_SystemParameters paraID)
        {
            var lastScannedId = systemParameterDataAccess.GetPreviousTimingExecution(paraID);

            return lastScannedId != null ? Int64.Parse(lastScannedId?.ParameterValue) : 0;
        }

        public UInt64 GetLastScannedId2(CTS_SystemParameters paraID)
        {
            var lastScannedId = systemParameterDataAccess.GetPreviousTimingExecution(paraID);

            return lastScannedId != null ? UInt64.Parse(lastScannedId?.ParameterValue) : 0;
        }

        public DateTime? GetLastScannedDateTime(CTS_SystemParameters paraID)
        {
            var lastScannedTransID = systemParameterDataAccess.GetPreviousTimingExecution(paraID);

            return lastScannedTransID != null ? DateTime.Parse(lastScannedTransID?.ParameterValue) : null;
        }

        public bool UpdateTimingExecution(string paraValue, CTS_SystemParameters paramId)
        {
            if (!string.IsNullOrEmpty(paraValue))
            {
                systemParameterDataAccess.UpdateTimingExecution(paraValue, paramId);
            }

            return true;
        }
    }
#pragma warning restore S6580
#pragma warning restore S2589 // Boolean expressions should not be gratuitous
}
