namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency
{
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Entitites;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert.Model;

    public interface INewCustomerClassificationAgencyDataAccess
    {
        NewCustomerClassificationAgencyModel GetNewCTSCustomerCategoryAgency();
    }

    public class NewCustomerClassificationAgencyDataAccess : INewCustomerClassificationAgencyDataAccess
    {
        private readonly IDynamicRepository _repository;

        public NewCustomerClassificationAgencyDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public NewCustomerClassificationAgencyModel GetNewCTSCustomerCategoryAgency()
        {
            var criteria = new NewCustomerCategoryAgencyGetCriteria();

            (var vvipClassification, var paClassification, var normalClassification) = _repository.FetchMultiple<VVIPCategoryAgencyEntity, InsertProblemAccountEntity, NormalRescanAgencyEntity>(criteria);

            return new NewCustomerClassificationAgencyModel
            {
                VVIPClassification = vvipClassification,
                PAClassification = paClassification,
                NormalClassification = normalClassification,
                LastCTSCustID = criteria.LastCTSCustID
            };
        }
    }
}
