namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency
{    
    using System.Collections.Generic;   

    public class ResultRemoveCustomerClassificationAgencyModel
    {
        public IEnumerable<ResultRemoveCustomerClassificationEntity> CustInfos { get; set; }

        public string ErrorMessage { get; set; }
    }
}
