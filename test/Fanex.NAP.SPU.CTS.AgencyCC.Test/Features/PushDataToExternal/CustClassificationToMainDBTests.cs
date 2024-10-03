using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Entities;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.AntiFraudCenter;
using Fanex.NAP.SPU.CTS.AgencyCC.Test.Infrastructure.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using static Fanex.NAP.SPU.CTS.AgencyCC.Test.Infrastructure.Consts.PushDataToExternal.CustClassificationToMainDBConsts;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Test.Features.PushDataToExternal
{
    public class CustClassificationToMainDBTests
    {
        private readonly ICustClassToMainDBDataAccess _dataAccess;

        private readonly ISentryClient _sentryClient;

        private readonly ICustClassToMainDBService _service;

        public CustClassificationToMainDBTests()
        {
            _sentryClient = Substitute.For<ISentryClient>();

            _dataAccess = Substitute.For<ICustClassToMainDBDataAccess>();
            _service = new CustClassToMainDBService(_dataAccess, _sentryClient);
        }

        [Fact]
        public void PushExternalPackageByCustId_WhenSuccess_DoesNotThrowException()
        {
            // Arrange
            var custIDs = new List<long>() { 1, 2, 3 };
            var parameter = new PushDataToExternalModel
            {
                ExternalBatchSize = 50,
                IsUpdateDangerToMainDB = true,
                IsWriteLog = false,
                ServiceName = "Test"
            };

            // Act
            var exception = Record.Exception(() => _service.PushExternalPackageByCustId(custIDs, parameter));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void PushCustomerClassification_WhenSuccess_DoesNotThrowException()
        {
            // Arrange
            var customerClass = new List<LatestClassificationModel>()
            {
                new LatestClassificationModel { CustId = 1, Category = 20200, CustomerClass = 10 }
            };

            // Act   
            _dataAccess.UpdateCustomerClassification(customerClass).Returns(new Shares.Models.DBResult());

            var exception = Record.Exception(() => _service.PushCustomerClassification(customerClass, 1, "Test"));

            // Assert
            Assert.Null(exception);
        }

        #region PushCustClassificationToMainDB
        [Fact]
        public void PushCustClassificationToMainDB_WhenSuccess()
        {
            // Arrange
            var data = new List<LatestCCDangerLevelEntity>
            {
                new LatestCCDangerLevelEntity()
            };

            var pushCustClassificationToMainDBMethod = TestExtension.GetPrivateMethod(typeof(CustClassToMainDBService), PrivateMethodName.PushCustClassificationToMainDB);


            // Act
            _dataAccess.UpdateCustomerClassification(Arg.Any<IEnumerable<LatestClassificationModel>>()).Returns(new Shares.Models.DBResult());

            var result = pushCustClassificationToMainDBMethod.Invoke(_service, new object[] { data, string.Empty });
            // Assert
            _dataAccess.Received(1).UpdateCustomerClassification(Arg.Any<IEnumerable<LatestClassificationModel>>());

            Assert.True((bool)result);
        }

        [Fact]
        public void PushCustClassificationToMainDB_WhenParamEmpty()
        {
            // Arrange
            var data = new List<LatestCCDangerLevelEntity>
            {
            };

            var pushCustClassificationToMainDBMethod = TestExtension.GetPrivateMethod(typeof(CustClassToMainDBService), PrivateMethodName.PushCustClassificationToMainDB);


            // Act
            var result = pushCustClassificationToMainDBMethod.Invoke(_service, new object[] { data, string.Empty });
            // Assert
            _dataAccess.Received(0).UpdateCustomerClassification(Arg.Any<IEnumerable<LatestClassificationModel>>());

            Assert.True((bool)result);
        }

        [Fact]
        public void PushCustClassificationToMainDB_ThrowException()
        {
            // Arrange
            var data = new List<LatestCCDangerLevelEntity>
            {
                new LatestCCDangerLevelEntity()
            };

            var pushCustClassificationToMainDBMethod = TestExtension.GetPrivateMethod(typeof(CustClassToMainDBService), PrivateMethodName.PushCustClassificationToMainDB);


            // Act
            _dataAccess.UpdateCustomerClassification(Arg.Any<IEnumerable<LatestClassificationModel>>()).Throws<Exception>();

            var result = pushCustClassificationToMainDBMethod.Invoke(_service, new object[] { data, string.Empty});
            // Assert
            _dataAccess.Received(4).UpdateCustomerClassification(Arg.Any<IEnumerable<LatestClassificationModel>>());

            Assert.False((bool)result);
        }
        #endregion

        #region GetLatestCCAndDangerLevel
        [Fact]
        public void GetLatestCCAndDangerLevel_WhenSuccess()
        {
            // Arrange
            var data = new List<long>() { 1, 2, 3, 4 };

            var getLatestCCAndDangerLevelMethod = TestExtension.GetPrivateMethod(typeof(CustClassToMainDBService), PrivateMethodName.GetLatestCCAndDangerLevel);

            // Act
            _dataAccess.GetLatestCCAndDangerLevel(Arg.Any<IEnumerable<long>>()).Returns(new List<LatestCCDangerLevelEntity>());

            var result = getLatestCCAndDangerLevelMethod.Invoke(_service, new object[] { data });

            // Assert
            _dataAccess.Received(1).GetLatestCCAndDangerLevel(Arg.Any<IEnumerable<long>>());

            Assert.NotNull(result);
        }

        [Fact]
        public void GetLatestCCAndDangerLevel_WhenParamEmpty()
        {
            // Arrange
            var getLatestCCAndDangerLevelMethod = TestExtension.GetPrivateMethod(typeof(CustClassToMainDBService), PrivateMethodName.GetLatestCCAndDangerLevel);

            // Act
            _dataAccess.GetLatestCCAndDangerLevel(Arg.Any<IEnumerable<long>>()).Returns(new List<LatestCCDangerLevelEntity>());

            var result = getLatestCCAndDangerLevelMethod.Invoke(_service, new object[] { Enumerable.Empty<long>() });

            // Assert
            _dataAccess.Received(0).GetLatestCCAndDangerLevel(Arg.Any<IEnumerable<long>>());

            Assert.Empty((IEnumerable<LatestCCDangerLevelEntity>)result);
        }

        [Fact]
        public void GetLatestCCAndDangerLevel_ThrowException()
        {
            // Arrange
            var data = new List<long>() { 1, 2, 3, 4 };

            var getLatestCCAndDangerLevelMethod = TestExtension.GetPrivateMethod(typeof(CustClassToMainDBService), PrivateMethodName.GetLatestCCAndDangerLevel);

            // Act
            _dataAccess.GetLatestCCAndDangerLevel(Arg.Any<IEnumerable<long>>()).Throws<Exception>();

            Action action = () => getLatestCCAndDangerLevelMethod.Invoke(_service, new object[] { data });

            // Assert
            _dataAccess.Received(0).GetLatestCCAndDangerLevel(Arg.Any<IEnumerable<long>>());

            Assert.Throws<TargetInvocationException>(action);
        }
        #endregion

        #region IsPushExternalSuccess
        [Fact]
        public void IsPushExternalSuccess_WhenSuccess()
        {
            // Arrange
            var lastestClassifications = new List<LatestCCDangerLevelEntity>
            {
                new LatestCCDangerLevelEntity() { DangerLevel = 1, DangerLevelType = 1 },
            };

            var paras = new PushDataToExternalModel() { IsUpdateDangerToMainDB = true };

            var isPushExternalSuccessMethod = TestExtension.GetPrivateMethod(typeof(CustClassToMainDBService), PrivateMethodName.IsPushExternalSuccess);

            // Act
            _dataAccess.UpdateCustomerClassification(Arg.Any<IEnumerable<LatestClassificationModel>>()).Returns(new Shares.Models.DBResult());

            //_dataAccess.UpdateDangerLevelToMainDB(Arg.Any<IEnumerable<DangerLevelEntity>>(), Arg.Any<int>()).Returns(new Shares.Models.DBResult());

            var result = isPushExternalSuccessMethod.Invoke(_service, new object[] { lastestClassifications, paras });
            // Assert
            //_dataAccess.Received(1).UpdateDangerLevelToMainDB(Arg.Any<IEnumerable<DangerLevelEntity>>(), Arg.Any<int>());

            _dataAccess.Received(1).UpdateCustomerClassification(Arg.Any<IEnumerable<LatestClassificationModel>>());

            Assert.True((bool)result);
        }

        [Fact]
        public void IsPushExternalSuccess_WhenSuccess_ParamEmpty()
        {
            // Arrange

            var paras = new PushDataToExternalModel() { IsUpdateDangerToMainDB = false };

            var isPushExternalSuccessMethod = TestExtension.GetPrivateMethod(typeof(CustClassToMainDBService), PrivateMethodName.IsPushExternalSuccess);

            // Act
            _dataAccess.UpdateCustomerClassification(Arg.Any<IEnumerable<LatestClassificationModel>>()).Returns(new Shares.Models.DBResult());

            var result = isPushExternalSuccessMethod.Invoke(_service, new object[] { Enumerable.Empty<LatestCCDangerLevelEntity>(), paras });
            // Assert
            //_dataAccess.Received(0).UpdateDangerLevelToMainDB(Arg.Any<IEnumerable<DangerLevelEntity>>(), Arg.Any<int>());

            _dataAccess.Received(0).UpdateCustomerClassification(Arg.Any<IEnumerable<LatestClassificationModel>>());

            Assert.True((bool)result);
        }
        #endregion
    }
}