namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter;
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
        }

        private static void RegisterDataAccesLayer(this IServiceCollection services)
        {
            services.AddSingleton<ISystemParameterServices, SystemParameterServices>();
            services.AddSingleton<ISystemParameterDataAccess, SystemParameterDataAccess>();
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
