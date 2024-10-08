using Fanex.NAP.SPU.CTS.AgencyCC.Features.DataLogTracking;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.DataLogTracking.Entities;
using NSubstitute;
using Sentry;
using System;
using Xunit;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Test.Features.DataLogTracking
{
    public class DataLogTrackingTest
    {
        private readonly IDataLogTrackingDataAccess _dataAccess;
        private readonly ISentryClient _sentryClient;

        private readonly IDataLogTrackingServices _service;

        public DataLogTrackingTest()
        {
            _dataAccess = Substitute.For<IDataLogTrackingDataAccess>();
            _sentryClient = Substitute.For<ISentryClient>();

            _service = new DataLogTrackingServices(_dataAccess, _sentryClient);
        }

        #region LogErrorData
        [Fact]
        public void LogErrorData_WhenSuccess()
        {
            // Arrange
            var exception = new Exception("TEST");

            // Act
            _dataAccess.LogErrorData(Arg.Any<LogErrorDataEntity>());

            _service.LogErrorData(exception, "TEST");

            // Assert
            _dataAccess.Received(1).LogErrorData(Arg.Any<LogErrorDataEntity>());
        }
        #endregion
    }
}
