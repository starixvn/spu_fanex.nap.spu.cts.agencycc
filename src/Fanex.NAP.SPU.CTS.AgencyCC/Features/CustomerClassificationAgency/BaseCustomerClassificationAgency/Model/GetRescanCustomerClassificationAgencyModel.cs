namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;
    using System.Collections.Generic;

    public class GetRescanCustomerClassificationAgencyModel
    {
        public IEnumerable<RescanCustomerClassificationAgencyEntity> CustInfos { get; set; }
    }
}
