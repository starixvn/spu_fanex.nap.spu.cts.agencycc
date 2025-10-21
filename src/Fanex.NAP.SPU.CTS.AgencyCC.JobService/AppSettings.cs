namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares.Models;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.ComponentModel;

    public interface IAppSettings
    {
        int Environment { get; }

        string EvironmentName { get; }

        string SPName { get; }

        string AFCInsertAccountURL { get; }

        string AFCDeleteAccountURL { get; }

        bool IsSyncAFCData { get; }     

        bool NormalAccountIsWriteLog { get; }

        bool ProblemAccountIsWriteLog { get; }

        string SentryDsn { get; }      

        string SentryCategory { get; }

        bool UpdateIsWriteLogSetting(string serviceName, bool isWriteLog);        

        APIHelperConfig APIHelperConfig { get; }
    }

    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        private readonly string environment;
        private readonly string evironmentName;
        private readonly string spName;
        private readonly string afcInsertAccountURL;
        private readonly string afcDeleteAccountURL;
        private readonly bool isSyncAFCData;
        private readonly string sentryDsn;       
        private readonly string sentryCategory;
        private readonly APIHelperConfig apiHelperConfig;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;

            environment = GetValue<string>(nameof(Environment));
            evironmentName = GetValue<string>(nameof(Environment));
            spName = GetValue<string>(nameof(SPName));
            sentryDsn = GetValue<string>("Sentry:Dsn");          
            sentryCategory = GetValue<string>("Sentry:Category");
            apiHelperConfig = new APIHelperConfig
            {
                Proxy = GetValue<string>("APIHelperConfig:Proxy")
            };
        }

        #region Variables

        public int Environment
        {
            get
            {
                switch (environment)
                {
                    case "LOCAL":
                        return 1;

                    case "UAT":
                        return 2;

                    default:
                        return 3;
                }
            }
        }

        public string EvironmentName => evironmentName;

        public string SPName => spName;

        public string AFCInsertAccountURL => afcInsertAccountURL;

        public string AFCDeleteAccountURL => afcDeleteAccountURL;

        public bool IsSyncAFCData => isSyncAFCData;

        public string SentryDsn => sentryDsn;

        public string SentryCategory => sentryCategory;

        public bool NormalAccountIsWriteLog => GetValue<bool>("NormalAccountInsert:IsWriteLog");

        public bool ProblemAccountIsWriteLog => GetValue<bool>("ProblemAccountInsert:IsWriteLog");

        public APIHelperConfig APIHelperConfig => apiHelperConfig;

        #endregion Variables

        #region Methods

        public T GetValue<T>(string key)
        {
            try
            {
                var value = configuration[key];

                var typeConverter = TypeDescriptor.GetConverter(typeof(T));

                return (T)typeConverter.ConvertFromString(value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Key: {key}", ex);
            }
        }

        public T GetSection<T>(string key)
        {
            var section = configuration.GetSection(key);

            return section.Get<T>();
        }

        public void SetValue<T>(string key, T value)
        {
            try
            {
                var typeConverter = TypeDescriptor.GetConverter(typeof(T));

                configuration[key] = typeConverter.ConvertToString(value);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Key: {key}", ex);
            }
        }

        public bool UpdateIsWriteLogSetting(string serviceName, bool isWriteLog)
        {
            var key = serviceName + ":IsWriteLog";

            if (configuration[key] == null)
            {
                return false;
            }

            try
            {
                SetValue<bool>(serviceName + ":IsWriteLog", isWriteLog);
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion Methods
    }
}