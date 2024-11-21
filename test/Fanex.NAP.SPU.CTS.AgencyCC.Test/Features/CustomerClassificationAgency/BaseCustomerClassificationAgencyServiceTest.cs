using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Shares.Models;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sentry;
using System;
using Xunit;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Features.CustomerClassificationAgency
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

        [Fact]
        public void RemoveCustomerClassificationAgency_WhenSuccess()
        {
            // Arrange
            var inputFlow = AgencyCCInputFlow.RemovePACategory;
            var modelRemove = new RemoveCustomerClassificationAgencyModel();

            var data = new ResultRemoveCustomerClassificationAgencyModel();

            // Act
            baseDataAccess.RemoveCustomerClassificationAgency(inputFlow, modelRemove).Returns(data);

            var result = baseService.RemoveCustomerClassificationAgency(inputFlow, modelRemove);

            // Assert
            baseDataAccess.Received(1).RemoveCustomerClassificationAgency(inputFlow, modelRemove);

            Assert.Equal(result, data);
        }

        [Fact]
        public void RemoveCustomerClassificationAgency_ThrowException()
        {
            // Arrange
            var inputFlow = AgencyCCInputFlow.RemovePACategory;
            var modelRemove = new RemoveCustomerClassificationAgencyModel();

            // Act
            baseDataAccess.RemoveCustomerClassificationAgency(inputFlow, modelRemove).Throws<Exception>();

            Action action = () => baseService.RemoveCustomerClassificationAgency(inputFlow, modelRemove);

            // Assert
            baseDataAccess.Received(0).RemoveCustomerClassificationAgency(inputFlow, modelRemove);

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void UpdateBetLimitOfFlaggedCustomer_WhenSuccess()
        {
            // Arrange

            // Act
            baseDataAccess.UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>()).Returns(new DBResult());

            var result = baseService.UpdateBetLimitOfFlaggedCustomer("ABC");

            // Assert
            baseDataAccess.Received(1).UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>());

            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateBetLimitOfFlaggedCustomer_ThrowException()
        {
            // Arrange

            // Act
            baseDataAccess.UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>()).Throws<Exception>();

            Action action = () => baseService.UpdateBetLimitOfFlaggedCustomer("ABC");

            // Assert
            baseDataAccess.Received(0).UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>());

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void UpdateBetLimitOfUnflaggedCustomer_WhenSuccess()
        {
            // Arrange

            // Act
            baseDataAccess.UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>()).Returns(new DBResult());

            var result = baseService.UpdateBetLimitOfUnflaggedCustomer("ABC");

            // Assert
            baseDataAccess.Received(1).UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>());

            Assert.NotNull(result);
        }

        [Fact]
        public void UpdateBetLimitOfUnflaggedCustomer_ThrowException()
        {
            // Arrange

            // Act
            baseDataAccess.UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>()).Throws<Exception>();

            Action action = () => baseService.UpdateBetLimitOfUnflaggedCustomer("ABC");

            // Assert
            baseDataAccess.Received(0).UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>());

            Assert.Throws<Exception>(action);
        }
    }
}
