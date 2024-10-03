namespace Fanex.NAP.SPU.CTS.AgencyCC.Test.Features.CustomerClassificationAgency.DailyNormalClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter;
    using Fanex.NAP.SPU.CTS.AgencyCC.Test.Infrastructure.Extensions;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Test.Infrastructure.Consts.CustomerClassificationAgency.DailyNormalClassificationAgencyConsts;

    public class DailyNormalClassificationAgencyServiceTest
    {
        private readonly IDailyNormalClassificationAgencyDataAccess _dataAccess;
        private readonly ISystemParameterServices _systemParameterService;
        private readonly IDailyNormalClassificationAgencyService _service;

        public DailyNormalClassificationAgencyServiceTest()
        {
            _dataAccess = Substitute.For<IDailyNormalClassificationAgencyDataAccess>();
            _systemParameterService = Substitute.For<ISystemParameterServices>();

            _service = new DailyNormalClassificationAgencyService(_dataAccess, _systemParameterService);
        }

        [Fact]
        public void GetCustomerForDailyNormalScanning_WhenSuccess()
        {
            // Arrange
            var scanningCustomers = new DailyNormalClassificationAgencyModel();

            var scanningCustomersPrivateField = TestExtension.GetPrivateFieldInfo(typeof(DailyNormalClassificationAgencyService), PrivateFieldName.SMADailyNormalClassificationModel);

            // Act
            _dataAccess.GetCustomerForDailyNormalScanning().Returns(scanningCustomers);

            _service.GetCustomerForDailyNormalScanning();

            var result = scanningCustomersPrivateField.GetValue(_service);

            // Assert
            _dataAccess.Received(1).GetCustomerForDailyNormalScanning();

            Assert.Equal(scanningCustomers, result);
        }

        [Fact]
        public void GetCustomerForDailyNormalScanning_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.GetCustomerForDailyNormalScanning().Throws<Exception>();

            Action action = () => _service.GetCustomerForDailyNormalScanning();

            // Assert
            _dataAccess.Received(0).GetCustomerForDailyNormalScanning();

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void PreprocessDailyNormalCustomer_WhenSuccess()
        {
            // Arrange
            var scanningCustomers = new DailyNormalClassificationAgencyModel()
            {
                CustID = new List<long>() { 1, 2, 3, 2 }
            };

            var scanningCustomersPrivateField = TestExtension.GetPrivateFieldInfo(typeof(DailyNormalClassificationAgencyService), PrivateFieldName.SMADailyNormalClassificationModel);

            // Act
            scanningCustomersPrivateField.SetValue(_service, scanningCustomers);

            _service.PreprocessDailyNormalCustomer();

            // Assert
            _dataAccess.Received(1).PreprocessNormalCustomer(Arg.Any<string>());
        }

        [Fact]
        public void PreprocessDailyNormalCustomer_ThrowException()
        {
            // Arrange
            var scanningCustomers = new DailyNormalClassificationAgencyModel()
            {
                CustID = new List<long>() { 1, 2, 3, 4 }
            };

            var scanningCustomersPrivateField = TestExtension.GetPrivateFieldInfo(typeof(DailyNormalClassificationAgencyService), PrivateFieldName.SMADailyNormalClassificationModel);

            // Act
            scanningCustomersPrivateField.SetValue(_service, scanningCustomers);

            _dataAccess.When(x => x.PreprocessNormalCustomer(Arg.Any<string>())).Throw<Exception>();

            Action action = () => _service.PreprocessDailyNormalCustomer();

            // Assert
            _dataAccess.Received(0).PreprocessNormalCustomer(Arg.Any<string>());

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void UpdateLastScan_WhenSuccess()
        {
            // Arrange
            var scanningCustomers = new DailyNormalClassificationAgencyModel()
            {
                LastCategoryID = 999,
                LastCustID = 1234
            };

            var scanningCustomersPrivateField = TestExtension.GetPrivateFieldInfo(typeof(DailyNormalClassificationAgencyService), PrivateFieldName.SMADailyNormalClassificationModel);

            // Act
            scanningCustomersPrivateField.SetValue(_service, scanningCustomers);

            _service.UpdateLastScan();

            // Assert
            _systemParameterService.Received(2).UpdateTimingExecution(Arg.Any<string>(), Arg.Any<CTS_SystemParameters>());
        }

        [Fact]
        public void UpdateLastScan_ThrowException()
        {
            // Arrange
            var scanningCustomers = new DailyNormalClassificationAgencyModel()
            {
                LastCategoryID = 999,
                LastCustID = 1234
            };

            var scanningCustomersPrivateField = TestExtension.GetPrivateFieldInfo(typeof(DailyNormalClassificationAgencyService), PrivateFieldName.SMADailyNormalClassificationModel);

            // Act
            scanningCustomersPrivateField.SetValue(_service, scanningCustomers);

            _systemParameterService.UpdateTimingExecution(Arg.Any<string>(), Arg.Any<CTS_SystemParameters>()).Throws<Exception>();

            Action action = () => _service.UpdateLastScan();

            // Assert
            _systemParameterService.Received(0).UpdateTimingExecution(Arg.Any<string>(), Arg.Any<CTS_SystemParameters>());

            Assert.Throws<Exception>(action);
        }
    }
}
