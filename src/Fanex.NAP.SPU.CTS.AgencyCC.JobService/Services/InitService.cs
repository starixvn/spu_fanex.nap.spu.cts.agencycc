namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency;
    using Microsoft.Extensions.DependencyInjection;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.AntiFraudCenter;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer;
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CTSCustomerAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency.AffectDownlineCustomerClassification;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.DataLogTracking;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification;

    public static class InitService
    {
        public static void RegisterService(this IServiceCollection services)
        {
            RegisterDataAccesLayer(services);

            RegisterCustomerAgency(services);

            RegisterRealtimeClassificationAgency(services);

            RegisterDailyNormalClassificationAgency(services);

            RegisterNormalAccountClassificationAgency(services);

            RegisterNormalAccountInsertAgency(services);

            RegisterNewCustomerClassificationAgency(services);

            RegisterProblemAccount(services);

            RegisterDataLogTracking(services);

            RegisterWrapperCustomerClassificaton(services);

            RegisterAffectDownline(services);
        }

        private static void RegisterAffectDownline(IServiceCollection services)
        {
            services.AddSingleton<IAffectedDownlineAgencyService, AffectedDownlineAgencyService>();
            services.AddSingleton<IAffectedDownlineAgencyJobService, AffectedDownlineAgencyJobService>();
        }

        private static void RegisterProblemAccount(IServiceCollection services)
        {
            services.AddSingleton<IProblemAccountAgencyService, ProblemAccountAgencyService>();
        }

        private static void RegisterDataLogTracking(IServiceCollection services)
        {
            services.AddSingleton<IDataLogTrackingDataAccess, DataLogTrackingDataAccess>();
            services.AddSingleton<IDataLogTrackingServices, DataLogTrackingServices>();
        }

        private static void RegisterNewCustomerClassificationAgency(IServiceCollection services)
        {
            services.AddSingleton<INewCustomerClassificationAgencyService, NewCustomerClassificationAgencyService>();
            services.AddSingleton<INewCustomerClassificationAgencyJobService, NewCustomerClassificationAgencyJobService>();
        }

        private static void RegisterCustomerAgency(IServiceCollection services)
        {
            services.AddSingleton<ICtsCustomerAgencyDataAccess, CtsCustomerAgencyDataAccess>();
            services.AddSingleton<ICtsCustomerAgencyService, CtsCustomerAgencyService>();
            services.AddSingleton<ICtsCustomerAgencyJobService, CtsCustomerAgencyJobService>();
        }

        private static void RegisterDataAccesLayer(IServiceCollection services)
        {
            services.AddSingleton<ISystemParameterServices, SystemParameterServices>();
            services.AddSingleton<ISystemParameterDataAccess, SystemParameterDataAccess>();

            services.AddSingleton<ICustClassToMainDBDataAccess, CustClassToMainDBDataAccess>();
            services.AddSingleton<ICustClassToMainDBService, CustClassToMainDBService>();

            services.AddSingleton<IRescanCustClassificationAgencyDataAccess, RescanCustClassificationAgencyDataAccess>();
            services.AddSingleton<IRescanCustClassificationAgencyService, RescanCustClassificationAgencyService>();

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
