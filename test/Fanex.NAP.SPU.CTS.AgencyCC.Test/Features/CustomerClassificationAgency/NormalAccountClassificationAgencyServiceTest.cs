namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Features.CustomerClassificationAgency.NormalAccountClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Model;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class NormalAccountClassificationAgencyServiceTest
    {
        private readonly INormalAccountClassificationAgencyDataAccess _dataAccess;
        private readonly INormalAccountClassificationAgencyService _service;

        public NormalAccountClassificationAgencyServiceTest()
        {
            _dataAccess = Substitute.For<INormalAccountClassificationAgencyDataAccess>();
            _service = new NormalAccountClassificationAgencyService(_dataAccess);
        }

        [Fact]
        public void GetNormalPoolCustomers_WhenSuccess()
        {
            // Arrange
            var normalPoolSMACustomer = new GetNormalPoolCustomerAgencyModel();

            // Act
            _dataAccess.GetNormalPoolCustomers().Returns(normalPoolSMACustomer);

            var result = _service.GetNormalPoolCustomers();

            // Assert
            _dataAccess.Received(1).GetNormalPoolCustomers();

            Assert.Equal(normalPoolSMACustomer, result);
        }

        [Fact]
        public void GetNormalPoolCustomers_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.GetNormalPoolCustomers().Throws<Exception>();

            Action action = () => _service.GetNormalPoolCustomers();

            // Assert
            _dataAccess.Received(0).GetNormalPoolCustomers();

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void GetCurrentCustomerCategory_WhenSuccess()
        {
            // Arrange
            var smaCategories = new List<GetCurrentNormalCategoryAgencyEntity>();

            // Act
            _dataAccess.GetCurrentCustomerCategory(Arg.Any<string>()).Returns(smaCategories);

            var result = _service.GetCurrentCustomerCategory(string.Empty);

            // Assert
            _dataAccess.Received(1).GetCurrentCustomerCategory(Arg.Any<string>());

            Assert.Equal(smaCategories, result);
        }

        [Fact]
        public void GetCurrentCustomerCategory_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.GetCurrentCustomerCategory(Arg.Any<string>()).Throws<Exception>();

            Action action = () => _service.GetCurrentCustomerCategory(string.Empty);

            // Assert
            _dataAccess.Received(0).GetCurrentCustomerCategory(Arg.Any<string>());

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void ClassifyNormalPoolCustomers_WhenSuccess()
        {
            // Arrange

            // Act
            _service.ClassifyNormalPoolCustomers(string.Empty);

            // Assert
            _dataAccess.Received(1).ClassifyNormalPoolCustomers(Arg.Any<string>());
        }

        [Fact]
        public void ClassifyNormalPoolCustomers_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.When(x => x.ClassifyNormalPoolCustomers(Arg.Any<string>())).Throw<Exception>();

            Action action = () => _service.ClassifyNormalPoolCustomers(string.Empty);

            // Assert
            _dataAccess.Received(0).ClassifyNormalPoolCustomers(Arg.Any<string>());

            Assert.Throws<Exception>(action);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ClearNormalPoolCustomers_WhenSuccess(long scannedMaxId)
        {
            // Arrange

            // Act
            _service.ClearNormalPoolCustomers(string.Empty, scannedMaxId);

            // Assert
            _dataAccess.Received(scannedMaxId > 0 ? 1 : 0).ClearNormalPoolCustomers(Arg.Any<string>(), Arg.Any<long>());
        }

        [Fact]
        public void ClearNormalPoolCustomers_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.When(x => x.ClearNormalPoolCustomers(Arg.Any<string>(), Arg.Any<long>())).Throw<Exception>();

            Action action = () => _service.ClearNormalPoolCustomers(string.Empty, 1);

            // Assert
            _dataAccess.Received(0).ClearNormalPoolCustomers(Arg.Any<string>(), Arg.Any<long>());

            Assert.Throws<Exception>(action);
        }
    }
}
