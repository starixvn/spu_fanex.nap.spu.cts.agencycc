using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
using Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using Xunit;
using static Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Consts.CustomerClassificationAgency.DailyProblemClassificationAgency.DailyProblemClassificationAgencyServiceConsts;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Features.CustomerClassificationAgency.DailyProblemClassificationAgency
{
    public class DailyProblemClassificationAgencyServiceTest
    {
        private readonly IProblemWLClassificationAgencyDataAccess _dataAccess;
        private readonly IProblemAccountAgencyService _problemAccountService;
        private readonly IServiceProvider _serviceProvider;

        private readonly IDailyProblemClassificationAgencyService _service;

        public DailyProblemClassificationAgencyServiceTest()
        {
            _dataAccess = Substitute.For<IProblemWLClassificationAgencyDataAccess>();
            _problemAccountService = Substitute.For<IProblemAccountAgencyService>();
            _serviceProvider = Substitute.For<IServiceProvider>();

            _service = new DailyProblemClassificationAgencyService(_dataAccess, _problemAccountService, _serviceProvider);
        }

        [Fact]
        public void Start_WhenSuccess()
        {
            // Arrange
            var paDailyScanFromQueueField = TestExtension.GetPrivateFieldInfo(typeof(DailyProblemClassificationAgencyService), PrivateFieldName.PADailyScanFromQueue);

            // Act
            _service.Start();

            var paDailyScanFromQueue = paDailyScanFromQueueField.GetValue(_service);
            // Assert
            Assert.NotNull(paDailyScanFromQueue);
        }

        [Fact]
        public void UpdateComplete_WhenSuccessWithCustIdsNotEmpty()
        {
            // Arrange
            var paDailyScanFromQueue = new List<long>() { 1, 2, 3 };

            var paDailyScanFromQueueField = TestExtension.GetPrivateFieldInfo(typeof(DailyProblemClassificationAgencyService), PrivateFieldName.PADailyScanFromQueue);

            // Act
            paDailyScanFromQueueField.SetValue(_service, paDailyScanFromQueue);

            var result = _service.UpdateComplete();

            // Assert
            _dataAccess.Received(1).CompletePADailyScan(Arg.Any<string>());
            Assert.True(result);
        }

        [Fact]
        public void UpdateComplete_WhenSuccessWithCustIdsEmpty()
        {
            // Arrange

            // Act
            var result = _service.UpdateComplete();

            // Assert
            _dataAccess.Received(1).CompletePADailyScan(Arg.Any<string>());
            Assert.False(result);
        }

        [Fact]
        public void UpdateComplete_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.When(action => action.CompletePADailyScan(Arg.Any<string>())).Throw<Exception>();

            Action action = () => _service.UpdateComplete();

            // Assert
            _dataAccess.Received(0).CompletePADailyScan(Arg.Any<string>());
            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void GetPADailyAgencyScanFromQueue_WhenSuccess()
        {
            // Arrange
            var paDailyScanFromQueue = new List<long>() { 1, 2, 3 };
            var paDailyScanFromQueueField = TestExtension.GetPrivateFieldInfo(typeof(DailyProblemClassificationAgencyService), PrivateFieldName.PADailyScanFromQueue);

            // Act
            _dataAccess.GetPADailyScanFromQueue().Returns(paDailyScanFromQueue);

            _service.Start().GetPADailyAgencyScanFromQueue();

            var paDailyScanFromQueueResult = paDailyScanFromQueueField.GetValue(_service);

            // Assert
            _dataAccess.Received(1).GetPADailyScanFromQueue();
            Assert.StrictEqual(paDailyScanFromQueue, paDailyScanFromQueueResult);
        }

        [Fact]
        public void GetPADailyAgencyScanFromQueue_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.GetPADailyScanFromQueue().Throws<Exception>();

            Action action = () => _service.Start().GetPADailyAgencyScanFromQueue();

            // Assert
            _dataAccess.Received(0).GetPADailyScanFromQueue();
            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void GetProblemAccountLosingClassification_WhenSuccessNotEmpty()
        {
            // Arrange
            var paDailyScanFromQueue = new List<long>() { 1, 2, 3 };
            var paLosings = new List<ProblemAccountLosingAgencyEntity> { new ProblemAccountLosingAgencyEntity() };
            var paDailyScanFromQueueField = TestExtension.GetPrivateFieldInfo(typeof(DailyProblemClassificationAgencyService), PrivateFieldName.PADailyScanFromQueue);


            // Act
            paDailyScanFromQueueField.SetValue(_service, paDailyScanFromQueue);
            _problemAccountService.GetProblemAccountLosingAgencies(Arg.Any<IEnumerable<long>>()).Returns(paLosings);

            var result = _service.GetProblemAccountLosingClassification();
            // Assert
            _problemAccountService.Received(1).GetProblemAccountLosingAgencies(Arg.Any<IEnumerable<long>>());
            Assert.StrictEqual(paLosings, result);
        }

        [Fact]
        public void GetProblemAccountLosingClassification_WhenSuccessEmpty()
        {
            // Arrange

            // Act
            var result = _service.GetProblemAccountLosingClassification();

            // Assert
            _problemAccountService.Received(0).GetProblemAccountLosingAgencies(Arg.Any<IEnumerable<long>>());
            Assert.Empty(result);
        }

        [Fact]
        public void GetProblemAccountLosingClassification_ThrowException()
        {
            // Arrange
            var paDailyScanFromQueue = new List<long>() { 1, 2, 3 };
            var paDailyScanFromQueueField = TestExtension.GetPrivateFieldInfo(typeof(DailyProblemClassificationAgencyService), PrivateFieldName.PADailyScanFromQueue);


            // Act
            paDailyScanFromQueueField.SetValue(_service, paDailyScanFromQueue);
            _problemAccountService.GetProblemAccountLosingAgencies(Arg.Any<IEnumerable<long>>()).Throws<Exception>();

            Action action = () => _service.GetProblemAccountLosingClassification();

            // Assert
            _problemAccountService.Received(0).GetProblemAccountLosingAgencies(Arg.Any<IEnumerable<long>>());
            Assert.Throws<Exception>(action);
        }
    }
}
