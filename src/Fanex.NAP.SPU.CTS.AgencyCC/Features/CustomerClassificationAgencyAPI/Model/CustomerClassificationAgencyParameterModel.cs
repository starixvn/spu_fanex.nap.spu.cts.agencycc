namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgencyAPI.Model
{
    public class CustomerClassificationAgencyParameterModel<T>
    {
        public T Data { get; set; }

        public int InternalBatchSize { get; set; }

        public int ExternalBatchSize { get; set; }
    }
}
