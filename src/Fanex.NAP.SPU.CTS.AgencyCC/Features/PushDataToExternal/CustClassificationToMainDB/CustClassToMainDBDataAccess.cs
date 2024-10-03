namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares.Models;

    public interface ICustClassToMainDBDataAccess
    {
        IEnumerable<LatestCCDangerLevelEntity> GetLatestCCAndDangerLevel(IEnumerable<long> custIds);

        DBResult UpdateCustomerClassification(IEnumerable<LatestClassificationModel> custClassification);

    }

    public class CustClassToMainDBDataAccess : ICustClassToMainDBDataAccess
    {
        private readonly IDynamicRepository _repository;

        public CustClassToMainDBDataAccess()
        {
            _repository = new DynamicRepository();
        }

        public IEnumerable<LatestCCDangerLevelEntity> GetLatestCCAndDangerLevel(IEnumerable<long> custIds)
        {
            var criteria = new GetLastestCCDangerLevelCriteria
            {
                CustIds = string.Join(",", custIds)
            };

            return _repository.Fetch<LatestCCDangerLevelEntity>(criteria) ?? Enumerable.Empty<LatestCCDangerLevelEntity>();
        }

        public DBResult UpdateCustomerClassification(IEnumerable<LatestClassificationModel> custClassification)
        {
            var criteria = new CustomerClassificationUpdateCriteria
            {
                custList = custClassification.ToXmlString(),
                //DataType = isSMACustomer ? 1 : null
            };

            _repository.Execute(criteria);

            return new DBResult
            {
                ErrorCode = criteria.ErrorCode,
                ErrorMsg = criteria.ErrorMessage
            };
        }
    }
}
