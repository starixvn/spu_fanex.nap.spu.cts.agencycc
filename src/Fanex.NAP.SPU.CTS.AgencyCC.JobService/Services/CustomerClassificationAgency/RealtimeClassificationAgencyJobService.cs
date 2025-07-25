namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using System;
    using System.Threading.Tasks;

    public interface IRealtimeClassificationAgencyJobService
    {
        Task<bool> Execute(int batchExternalSize, int batchInternalSize);
    }

    public class RealtimeClassificationAgencyJobService : IRealtimeClassificationAgencyJobService
    {
        private readonly IRealtimeClassificationAgencyService _service;
        private readonly string serviceName = "RealtimeClassificationAgency";

        public RealtimeClassificationAgencyJobService(IRealtimeClassificationAgencyService service)
        {
            _service = service;
        }

        public Task<bool> Execute(int batchExternalSize, int batchInternalSize)
        {
            try
            {
                if (batchExternalSize > 0 && batchInternalSize > 0)
                {
                    _service.Start()
                        .GetNewPlaceBetCustomers()
                        .GetCustomerCategory(batchInternalSize)
                        .PreprocessCustomerClassRealtime(batchExternalSize)
                        .InsertToQueuePaAgencyRealtime(batchExternalSize)
                        .InsertToQueueConsiderableDanger(batchExternalSize)
                        .CompleteCustomerRealtime();
                }
                else
                {
                    Utilities.LogSentryError(serviceName, "InValid Batch size", (batchExternalSize, batchInternalSize), new NullReferenceException());
                }
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(serviceName, ex.Message, string.Empty, ex);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
