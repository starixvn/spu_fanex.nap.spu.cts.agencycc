namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.AntiFraudCenter;
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency;
    using Microsoft.Extensions.DependencyInjection;

    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterDataAccesLayer(services);

            RegisterRealtimeClassificationAgency(services);

            RegisterDailyNormalClassificationAgency(services);

            RegisterNormalAccountClassificationAgency(services);

            RegisterNormalAccountInsertAgency(services);

            RegisterWrapperCustomerClassificaton(services);
        }

        private static void RegisterDataAccesLayer(this IServiceCollection services)
        {
            services.AddSingleton<ISystemParameterServices, SystemParameterServices>();
            services.AddSingleton<ISystemParameterDataAccess, SystemParameterDataAccess>();

            services.AddSingleton<ICustClassToMainDBDataAccess, CustClassToMainDBDataAccess>();
            services.AddSingleton<ICustClassToMainDBService, CustClassToMainDBService>();

            services.AddSingleton<IAntiFraudCenterServices, AntiFraudCenterServices>();
        }

        private static void RegisterWrapperCustomerClassificaton(IServiceCollection services)
        {
            services.AddSingleton<IBaseCustomerClassificationAgencyDataAccess, BaseCustomerClassificationAgencyDataAccess>();
            services.AddSingleton<IBaseCustomerClassificationAgencyService, BaseCustomerClassificationAgencyService>();
        }

        private static void RegisterNormalAccountInsertAgency(IServiceCollection services)
        {
            services.AddSingleton<INormalAccountInsertAgencyDataAccess, NormalAccountInsertAgencyDataAccess>();
            services.AddSingleton<INormalAccountInsertAgencyService, NormalAccountInsertAgencyService>();
            services.AddSingleton<INormalAccountInsertAgencyJobService, NormalAccountInsertAgencyJobService>();
        }

        private static void RegisterRealtimeClassificationAgency(IServiceCollection services)
        {
            services.AddSingleton<IRealtimeClassificationAgencyDataAccess, RealtimeClassificationAgencyDataAccess>();
            services.AddSingleton<IRealtimeClassificationAgencyService, RealtimeClassificationAgencyService>();
            services.AddSingleton<IRealtimeClassificationAgencyJobService, RealtimeClassificationAgencyJobService>();
        }

        private static void RegisterDailyNormalClassificationAgency(IServiceCollection services)
        {
            services.AddSingleton<IDailyNormalClassificationAgencyDataAccess, DailyNormalClassificationAgencyDataAccess>();
            services.AddSingleton<IDailyNormalClassificationAgencyService, DailyNormalClassificationAgencyService>();
            services.AddSingleton<IDailyNormalClassificationAgencyJobService, DailyNormalClassificationAgencyJobService>();
        }

        private static void RegisterNormalAccountClassificationAgency(IServiceCollection services)
        {
            services.AddSingleton<INormalAccountClassificationAgencyDataAccess, NormalAccountClassificationAgencyDataAccess>();
            services.AddSingleton<INormalAccountClassificationAgencyService, NormalAccountClassificationAgencyService>();
            services.AddSingleton<INormalAccountClassificationAgencyJobService, NormalAccountClassificationAgencyJobService>();
        }
    }
}
