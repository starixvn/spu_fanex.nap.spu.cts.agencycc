namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.DataLogTracking
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.DataLogTracking.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Sentry;
    using System;
    using System.Linq;

    public interface IDataLogTrackingServices
    {
        void LogErrorData(Exception exception, string serviceName);
    }

    public class DataLogTrackingServices : IDataLogTrackingServices
    {
        private readonly IDataLogTrackingDataAccess dataLogTrackingDataAccess;
        private readonly ISentryClient sentryClient;

        public DataLogTrackingServices(IDataLogTrackingDataAccess dataLogTrackingDataAccess, ISentryClient sentryClient)
        {
            this.dataLogTrackingDataAccess = dataLogTrackingDataAccess;
            this.sentryClient = sentryClient;
        }

        public void LogErrorData(Exception exception, string serviceName)
        {
            try
            {
                var logInfo = new LogErrorDataEntity
                {
                    Message = new LogMessage(exception.Message)
                                   .AddMessage($"{serviceName}")
                                   .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                                   .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                                   .AddMessage($"{ DateTime.Now}").ToString(),
                    FromFlow = serviceName,
                    SPName = DatabaseInfoGetters.GetDbExceptionInfo(exception)?.Where(c => c.Key == "SpName")?.FirstOrDefault().Value?.ToString(),
                    Parameters = DatabaseInfoGetters.GetDbExceptionInfo(exception)?.Where(c => c.Key == "Parameters")?.FirstOrDefault().Value?.ToString(),
                };

                dataLogTrackingDataAccess.LogErrorData(logInfo);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, "InsertErrorData fail", exception, ex, sentryClient);
            }
        }
    }
}
