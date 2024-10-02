namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Model;
    using System.Collections.Generic;

    public interface INormalAccountClassificationAgencyService
    {
        GetNormalPoolCustomerAgencyModel GetNormalPoolCustomers();

        IEnumerable<GetCurrentNormalCategoryAgencyEntity> GetCurrentCustomerCategory(string custIDList);

        INormalAccountClassificationAgencyService ClassifyNormalPoolCustomers(string customerXML);

        INormalAccountClassificationAgencyService ClearNormalPoolCustomers(string custIdList, long scannedMaxId);
    }

    public class NormalAccountClassificationAgencyService : INormalAccountClassificationAgencyService
    {
        private readonly INormalAccountClassificationAgencyDataAccess _dataAccess;

        public NormalAccountClassificationAgencyService(INormalAccountClassificationAgencyDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public GetNormalPoolCustomerAgencyModel GetNormalPoolCustomers()
        {
            return _dataAccess.GetNormalPoolCustomers();
        }

        public IEnumerable<GetCurrentNormalCategoryAgencyEntity> GetCurrentCustomerCategory(string custIDList)
        {
            return _dataAccess.GetCurrentCustomerCategory(custIDList);
        }

        public INormalAccountClassificationAgencyService ClassifyNormalPoolCustomers(string customerXML)
        {
            _dataAccess.ClassifyNormalPoolCustomers(customerXML);

            return this;
        }

        public INormalAccountClassificationAgencyService ClearNormalPoolCustomers(string custIdList, long scannedMaxId)
        {
            if (scannedMaxId > 0)
            {
                _dataAccess.ClearNormalPoolCustomers(custIdList, scannedMaxId);
            }

            return this;
        }
    }
}
