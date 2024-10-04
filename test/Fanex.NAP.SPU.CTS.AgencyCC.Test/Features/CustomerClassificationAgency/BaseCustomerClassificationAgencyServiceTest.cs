using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
using FluentAssertions;
using NSubstitute;
using Sentry;
using Xunit;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Test.Features.CustomerClassificationAgency
{
    public class BaseCustomerClassificationAgencyServiceTest
    {
        private readonly IBaseCustomerClassificationAgencyDataAccess baseDataAccess;
        private readonly IBaseCustomerClassificationAgencyService baseService;
        private readonly ISentryClient sentryClient;

        public BaseCustomerClassificationAgencyServiceTest()
        {
            sentryClient = Substitute.For<ISentryClient>();
            baseDataAccess = Substitute.For<IBaseCustomerClassificationAgencyDataAccess>();
            baseService = new BaseCustomerClassificationAgencyService(baseDataAccess, sentryClient);
        }


        [Fact]
        public void GetRescanCustomerClassification_WhenSuccess_ReturnData()
        {
            // Arrange
            var custIDs = "1,2";
            var expectedData = new GetRescanCustomerClassificationAgencyModel();

            // Act
            baseDataAccess.GetRescanCustomerClassificationAgency(custIDs).Returns(expectedData);

            var result = baseService.GetRescanCustomerClassificationAgency(custIDs);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void InsertCustomerClassification_WhenSuccess_ReturnData()
        {
            // Arrange
            var inputFlow = AgencyCCInputFlow.InsertPACategory;
            var modelInsert = new InsertCustomerClassificationAgencyModel();
            var serviceName = "Test";
            var expectedData = new ResultInsertCustomerClassificationAgencyModel();

            // Act
            baseDataAccess.InsertCustomerClassification(inputFlow, modelInsert).Returns(expectedData);

            var result = baseService.InsertCustomerClassification(inputFlow, modelInsert, serviceName);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void InsertCustomerClassification_WhenFail_ReturnErrorMsg()
        {
            // Arrange
            var inputFlow = AgencyCCInputFlow.InsertPACategory;
            var modelInsert = new InsertCustomerClassificationAgencyModel();
            var serviceName = "Test";
            var expectedData = new ResultInsertCustomerClassificationAgencyModel() { CustInfos = null, ErrorMessage = "Fail" };

            // Act
            baseDataAccess.InsertCustomerClassification(inputFlow, modelInsert).Returns(expectedData);

            var result = baseService.InsertCustomerClassification(inputFlow, modelInsert, serviceName);

            // Assert
            result.ErrorMessage.Equals(expectedData.ErrorMessage);
        }
    }
}
