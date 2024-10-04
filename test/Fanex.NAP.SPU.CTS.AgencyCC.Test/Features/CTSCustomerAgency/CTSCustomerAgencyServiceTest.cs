using Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CTSCustomer.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;
using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Test.Features.CTSCustomerAgency
{
    public class CTSCustomerAgencyServiceTest
    {
        private readonly ICTSCustomerAgencyDataAccess _dataAccess;
        private readonly ICTSCustomerAgencyService _service;

        public CTSCustomerAgencyServiceTest()
        {
            _dataAccess = Substitute.For<ICTSCustomerAgencyDataAccess>();
            _service = new CTSCustomerAgencyService(_dataAccess);
        }

        [Fact]
        public void GetInactivateNewCustomers_WhenSuccess_ReturnData()
        {
            var dataFromDB = new InactivateNewCustomerModel
            {
                CustInfos = new List<RescanCategoryDailyModel>
                {
                    new RescanCategoryDailyModel { CustID = 1, CurrentCategoryID = 201} ,
                    new RescanCategoryDailyModel { CustID = 2, CurrentCategoryID = 202}
                },
                LastCustID = 1,
                LastCategoryID = 1
            };

            _dataAccess.GetInactivateNewCustomers().Returns(dataFromDB);

            var realData = _service.GetInactivateNewCustomers();
            realData.Should().BeEquivalentTo(dataFromDB);
        }
    }
}
