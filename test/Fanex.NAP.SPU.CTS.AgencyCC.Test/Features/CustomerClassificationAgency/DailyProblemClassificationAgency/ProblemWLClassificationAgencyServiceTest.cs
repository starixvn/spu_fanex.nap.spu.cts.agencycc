using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyProblemClassificationAgency.Entities;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using Xunit;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;
using static Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Consts.CustomerClassificationAgency.DailyProblemClassificationAgency.ProblemWLClassificationAgencyServiceConsts;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Features.CustomerClassificationAgency.DailyProblemClassificationAgency
{
    public class ProblemWLClassificationAgencyServiceTest
    {
        private readonly IProblemWLClassificationAgencyDataAccess _dataAccess;
        private readonly ICustClassToMainDBService _custClassToMainDB;

        private readonly IProblemWLClassificationAgencyService _service;

        public ProblemWLClassificationAgencyServiceTest()
        {
            _dataAccess = Substitute.For<IProblemWLClassificationAgencyDataAccess>();
            _custClassToMainDB = Substitute.For<ICustClassToMainDBService>();

            _service = new ProblemWLClassificationAgencyService(_dataAccess, _custClassToMainDB);
        }

        [Fact]
        public void InsertPAWLClassificationAgency_WhenSuccessNotEmpty()
        {
            // Arrange
            var paWLClassification = new List<ProblemAccountLosingAgencyEntity> { new ProblemAccountLosingAgencyEntity() };
            var paWLsValue = new List<PAWLInsertAgencyEntity> { new PAWLInsertAgencyEntity() };
            var paWLsField = TestExtension.GetPrivateFieldInfo(typeof(ProblemWLClassificationAgencyService), PrivateFieldName.PAWLs);

            // Act
            _dataAccess.InsertProblemAccountWLPerformance(Arg.Any<string>()).Returns(paWLsValue);

            _service.InsertPAWLClassificationAgency(5000, paWLClassification);

            var paWLsResult = paWLsField.GetValue(_service);

            // Assert
            _dataAccess.Received(1).InsertProblemAccountWLPerformance(Arg.Any<string>());
            Assert.StrictEqual(paWLsValue, paWLsResult);
        }

        [Fact]
        public void InsertPAWLClassificationAgency_WhenSuccessEmpty()
        {
            // Arrange
            var paWLsField = TestExtension.GetPrivateFieldInfo(typeof(ProblemWLClassificationAgencyService), PrivateFieldName.PAWLs);

            // Act
            _service.InsertPAWLClassificationAgency(5000, null);

            var paWLsResult = paWLsField.GetValue(_service);

            // Assert
            _dataAccess.Received(0).InsertProblemAccountWLPerformance(Arg.Any<string>());
            Assert.NotNull(paWLsResult);
        }

        [Fact]
        public void InsertPAWLClassificationAgency_ThrowException()
        {
            // Arrange
            var paWLClassification = new List<ProblemAccountLosingAgencyEntity> { new ProblemAccountLosingAgencyEntity() };
            var paWLsField = TestExtension.GetPrivateFieldInfo(typeof(ProblemWLClassificationAgencyService), PrivateFieldName.PAWLs);

            // Act
            _dataAccess.InsertProblemAccountWLPerformance(Arg.Any<string>()).Throws<Exception>();

            Action action = () => _service.InsertPAWLClassificationAgency(5000, paWLClassification);

            var paWLsResult = paWLsField.GetValue(_service);

            // Assert
            _dataAccess.Received(0).InsertProblemAccountWLPerformance(Arg.Any<string>());
            Assert.NotNull(paWLsResult);
            Assert.Throws<Exception>(action);
        }

#pragma warning disable S125
        // not handle SMA
        //[Fact]
        //public void InsertPALogicAgency_WhenSuccess()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact]
        //public void InsertPALogicAgency_ThrowException()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}
#pragma warning disable S125

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PushCCToExternal_WhenSuccessNotEmpty(bool resultExpect)
        {
            // Arrange
            var paWLsValue = new List<PAWLInsertAgencyEntity> { new PAWLInsertAgencyEntity() };
            var paWLsField = TestExtension.GetPrivateFieldInfo(typeof(ProblemWLClassificationAgencyService), PrivateFieldName.PAWLs);

            var paras = new PushDataToExternalModel
            {
                RoleGroup = RoleGroupType.Agency,
                ExternalBatchSize = 9999,
                IsUpdateDangerToMainDB = false,
                IsWriteLog = false,
                ServiceName = "TEST"
            };

            // Act
            paWLsField.SetValue(_service, paWLsValue);
            _custClassToMainDB.PushExternalPackageByCustId(Arg.Any<IEnumerable<long>>(), paras).Returns(resultExpect);

            var result = _service.PushCCToExternal(paras);

            // Assert
            _custClassToMainDB.Received(1).PushExternalPackageByCustId(Arg.Any<IEnumerable<long>>(), paras);
            Assert.StrictEqual(resultExpect, result);
        }

        [Fact]
        public void PushCCToExternal_WhenSuccessEmpty()
        {
            // Arrange
            var paras = new PushDataToExternalModel
            {
                RoleGroup = RoleGroupType.Agency,
                ExternalBatchSize = 9999,
                IsUpdateDangerToMainDB = false,
                IsWriteLog = false,
                ServiceName = "TEST"
            };

            // Act
            var result = _service.PushCCToExternal(paras);

            // Assert
            _custClassToMainDB.Received(0).PushExternalPackageByCustId(Arg.Any<IEnumerable<long>>(), paras);
            Assert.True(result);
        }

        [Fact]
        public void PushCCToExternal_ThrowException()
        {
            // Arrange
            var paWLsValue = new List<PAWLInsertAgencyEntity> { new PAWLInsertAgencyEntity() };
            var paWLsField = TestExtension.GetPrivateFieldInfo(typeof(ProblemWLClassificationAgencyService), PrivateFieldName.PAWLs);

            var paras = new PushDataToExternalModel
            {
                RoleGroup = RoleGroupType.Agency,
                ExternalBatchSize = 9999,
                IsUpdateDangerToMainDB = false,
                IsWriteLog = false,
                ServiceName = "TEST"
            };

            // Act
            paWLsField.SetValue(_service, paWLsValue);
            _custClassToMainDB.PushExternalPackageByCustId(Arg.Any<IEnumerable<long>>(), paras).Throws<Exception>();

            Action action = () => _service.PushCCToExternal(paras);

            // Assert
            _custClassToMainDB.Received(0).PushExternalPackageByCustId(Arg.Any<IEnumerable<long>>(), paras);
            Assert.Throws<Exception>(action);
        }
    }
}
