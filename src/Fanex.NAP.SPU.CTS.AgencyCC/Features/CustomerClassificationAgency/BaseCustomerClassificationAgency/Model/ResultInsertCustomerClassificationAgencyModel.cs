namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model
{
    using System.Collections.Generic;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;

    public class ResultInsertCustomerClassificationAgencyModel
    {
        public IEnumerable<CustInfoClassificationAgencyEntity> CustInfos { get; set; }

        public string ErrorMessage { get; set; }
    }
}
