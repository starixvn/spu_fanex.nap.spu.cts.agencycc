namespace Fanex.NAP.SPU.CTS.AgencyCC.Test.Features.CustomerClassificationAgency.RealtimeClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RealtimeClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Test.Infrastructure.Extensions;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Test.Infrastructure.Consts.CustomerClassificationAgency.CustomerClassificationAgencyServiceConsts;

    public class RealtimeClassificationAgencyServiceTest
    {
        private readonly IRealtimeClassificationAgencyDataAccess _dataAccess;
        private readonly IRealtimeClassificationAgencyService _service;
        private GetNewPlaceBetCustomerAgencyModel _customer;

        public RealtimeClassificationAgencyServiceTest()
        {
            _dataAccess = Substitute.For<IRealtimeClassificationAgencyDataAccess>();
            _service = new RealtimeClassificationAgencyService(_dataAccess);

            InitialData();
        }

        private void InitialData()
        {
            _customer = new GetNewPlaceBetCustomerAgencyModel
            {
                CustomerNewPlaceBets = new List<GetNewPlaceBetCustomerAgencyEntity> {
                    new GetNewPlaceBetCustomerAgencyEntity
                    {
                        CustId = 1,
                        RoleId = 2
                    },
                    new GetNewPlaceBetCustomerAgencyEntity
                    {
                        CustId = 2,
                        RoleId = 3
                    },
                    new GetNewPlaceBetCustomerAgencyEntity
                    {
                        CustId = 3,
                        RoleId = 4
                    },
                    new GetNewPlaceBetCustomerAgencyEntity
                    {
                        CustId = 4,
                        RoleId = 2
                    }
                },
                ScannedMaxId = 1234
            };
        }

        [Fact]
        public void GetNewPlaceBetCustomers_WhenSuccess()
        {
            // Arrange
            var customers = _customer;

            var customersPrivateField = TestExtension.GetPrivateFieldInfo(typeof(RealtimeClassificationAgencyService), PrivateFieldName.GetNewPlaceBetSMACustomerModel);

            // Act
            _dataAccess.GetNewPlaceBetCustomers().Returns(customers);

            _service.GetNewPlaceBetCustomers();

            var result = customersPrivateField.GetValue(_service);

            // Assert
            _dataAccess.Received(1).GetNewPlaceBetCustomers();

            Assert.Equal(customers, result);
        }

        [Fact]
        public void GetNewPlaceBetCustomers_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.When(x => x.GetNewPlaceBetCustomers()).Throw<Exception>();

            Action action = () => _service.GetNewPlaceBetCustomers();

            // Assert
            _dataAccess.Received(0).GetNewPlaceBetCustomers();

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void GetCustomerCategory_WhenSuccess()
        {
            // Arrange
            var customers = _customer;

            var customersPrivateField = TestExtension.GetPrivateFieldInfo(typeof(RealtimeClassificationAgencyService), PrivateFieldName.GetNewPlaceBetSMACustomerModel);
            var customerCategoriesPrivateField = TestExtension.GetPrivateFieldInfo(typeof(RealtimeClassificationAgencyService), PrivateFieldName.GetSMACustomerCategoryRealtimeModel);

            // Act
            customersPrivateField.SetValue(_service, customers);
            customerCategoriesPrivateField.SetValue(_service, new GetCustomerCategoryAgencyRealtimeModel());

            _dataAccess.GetCustomerCategory(Arg.Any<string>()).Returns(new GetCustomerCategoryAgencyRealtimeModel
            {
                CustomerCategory = new List<long> { 1, 2 }
            });

            _service.GetCustomerCategory(2);

            var result = (GetCustomerCategoryAgencyRealtimeModel)customerCategoriesPrivateField.GetValue(_service);

            // Assert
            _dataAccess.Received(2).GetCustomerCategory(Arg.Any<string>());

            Assert.NotNull(result.CustomerCategory);
        }

        [Fact]
        public void GetCustomerCategory_ThrowException()
        {
            // Arrange
            var customers = _customer;

            var customersPrivateField = TestExtension.GetPrivateFieldInfo(typeof(RealtimeClassificationAgencyService), PrivateFieldName.GetNewPlaceBetSMACustomerModel);

            // Act
            customersPrivateField.SetValue(_service, customers);

            _dataAccess.GetCustomerCategory(Arg.Any<string>()).Throws<Exception>();

            Action action = () => _service.GetCustomerCategory(2);

            // Assert
            _dataAccess.Received(0).GetCustomerCategory(Arg.Any<string>());
            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void PreprocessCustomerClassRealtime_WhenSuccess()
        {
            // Arrange
            var customerCategories = new GetCustomerCategoryAgencyRealtimeModel
            {
                CustomerCategory = new List<long> { 1, 2, 3, 4 }
            };


            var customerCategoriesPrivateField = TestExtension.GetPrivateFieldInfo(typeof(RealtimeClassificationAgencyService), PrivateFieldName.GetSMACustomerCategoryRealtimeModel);

            // Act
            customerCategoriesPrivateField.SetValue(_service, customerCategories);

            _service.PreprocessCustomerClassRealtime(2);

            // Assert
            _dataAccess.Received(2).PreprocessCustomerClassRealtime(Arg.Any<string>());
        }

        [Fact]
        public void PreprocessCustomerClassRealtime_ThrowException()
        {
            // Arrange
            var customerCategories = new GetCustomerCategoryAgencyRealtimeModel
            {
                CustomerCategory = new List<long> { 1, 2, 3, 4 }
            };


            var customerCategoriesPrivateField = TestExtension.GetPrivateFieldInfo(typeof(RealtimeClassificationAgencyService), PrivateFieldName.GetSMACustomerCategoryRealtimeModel);

            // Act
            customerCategoriesPrivateField.SetValue(_service, customerCategories);

            _dataAccess.When(x => x.PreprocessCustomerClassRealtime(Arg.Any<string>())).Throw<Exception>();

            Action action = () => _service.PreprocessCustomerClassRealtime(2);

            // Assert
            _dataAccess.Received(0).PreprocessCustomerClassRealtime(Arg.Any<string>());

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void CompleteCustomerRealtime_WhenSuccess()
        {
            // Arrange
            var customers = new GetNewPlaceBetCustomerAgencyModel
            {
                ScannedMaxId = 1234
            };

            var customersPrivateField = TestExtension.GetPrivateFieldInfo(typeof(RealtimeClassificationAgencyService), PrivateFieldName.GetNewPlaceBetSMACustomerModel);

            // Act
            customersPrivateField.SetValue(_service, customers);

            _service.CompleteCustomerRealtime();

            // Assert
            _dataAccess.Received(1).CompleteCustomerRealtime(Arg.Any<long>());
        }

        [Fact]
        public void CompleteCustomerRealtime_ThrowException()
        {
            // Arrange
            var customers = new GetNewPlaceBetCustomerAgencyModel
            {
                ScannedMaxId = 1234
            };

            var customersPrivateField = TestExtension.GetPrivateFieldInfo(typeof(RealtimeClassificationAgencyService), PrivateFieldName.GetNewPlaceBetSMACustomerModel);

            // Act
            customersPrivateField.SetValue(_service, customers);

            _dataAccess.When(x => x.CompleteCustomerRealtime(Arg.Any<long>())).Throw<Exception>();

            Action action = () => _service.CompleteCustomerRealtime();

            // Assert
            _dataAccess.Received(0).CompleteCustomerRealtime(Arg.Any<long>());

            Assert.Throws<Exception>(action);
        }
    }
}
