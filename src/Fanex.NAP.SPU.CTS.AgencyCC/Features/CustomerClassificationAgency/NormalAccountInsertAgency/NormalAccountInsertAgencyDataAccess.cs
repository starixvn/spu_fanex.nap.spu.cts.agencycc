namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;

    public interface INormalAccountInsertAgencyDataAccess
    {
        GetNormalAccountAgencyModel GetNormalAccount();

        void CompleteNormalAccountInsert(long scannedMaxId);
    }

    public class NormalAccountInsertAgencyDataAccess : INormalAccountInsertAgencyDataAccess
    {
        private readonly IDynamicRepository _repository;

        public NormalAccountInsertAgencyDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public GetNormalAccountAgencyModel GetNormalAccount()
        {
            var criteria = new GetNormalAccountAgencyCriteria();

            var data = _repository.Fetch<GetNormalAccountAgencyEntity>(criteria);

            return new GetNormalAccountAgencyModel
            {
                NormalAccount = data,
                IsContinute = data.IsNotNullOrEmpty(),
                ScannedMaxId = criteria.ScannedMaxId
            };
        }

        public void CompleteNormalAccountInsert(long scannedMaxId)
        {
            var criteria = new InsertCompleteNormalAccountAgencyCrieria
            {
                ScannedMaxId = scannedMaxId
            };

            _repository.Execute(criteria);
        }
    }
}
