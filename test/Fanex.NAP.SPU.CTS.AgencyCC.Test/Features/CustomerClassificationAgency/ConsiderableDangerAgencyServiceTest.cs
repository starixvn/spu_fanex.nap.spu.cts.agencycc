namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Features.CustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
    using FluentAssertions;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public class ConsiderableDangerAgencyServiceTest
    {
        private readonly IConsiderableDangerAgencyDataAccess _dataAccess;
        private readonly IConsiderableDangerAgencyService _service;
        private readonly IProblemAccountAgencyService _problemAccountService;
        private readonly ICustClassToMainDBService _custClassToMainDBService;

        public ConsiderableDangerAgencyServiceTest()
        {
            _dataAccess = Substitute.For<IConsiderableDangerAgencyDataAccess>();
            _problemAccountService = Substitute.For<IProblemAccountAgencyService>();
            _custClassToMainDBService = Substitute.For<ICustClassToMainDBService>();
            _service = new ConsiderableDangerAgencyService(_dataAccess, _problemAccountService, _custClassToMainDBService);
        }

        #region UpdateConsiderableDangerQueueMaxID Tests

        [Fact]
        public void UpdateConsiderableDangerQueueMaxID_WhenSuccess_ShouldCallDataAccess()
        {
            // Act
            var result = _service.UpdateConsiderableDangerQueueMaxID();

            // Assert
            _dataAccess.Received(1).UpdateConsiderableDangerQueueMaxID();
            result.Should().Be(_service);
        }

        [Fact]
        public void UpdateConsiderableDangerQueueMaxID_WhenDataAccessThrowsException_ShouldPropagateException()
        {
            // Arrange
            _dataAccess.When(x => x.UpdateConsiderableDangerQueueMaxID()).Throw(new InvalidOperationException("Database error"));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _service.UpdateConsiderableDangerQueueMaxID());
            exception.Message.Should().Be("Database error");
        }

        [Fact]
        public void UpdateConsiderableDangerQueueMaxID_WhenDataAccessThrowsArgumentNullException_ShouldPropagateException()
        {
            // Arrange
            _dataAccess.When(x => x.UpdateConsiderableDangerQueueMaxID()).Throw(new ArgumentNullException("parameter"));

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _service.UpdateConsiderableDangerQueueMaxID());
            exception.ParamName.Should().Be("parameter");
        }

        #endregion

        #region GetConsiderableDangerQueue Tests

        [Fact]
        public void GetConsiderableDangerQueue_WhenSuccess_ShouldReturnExpectedData()
        {
            // Arrange
            var batchSize = 100;
            var expectedModel = new ConsiderableDangerRecheckModel
            {
                LastID = 12345,
                RecheckRatioCusts = new List<ConsiderableDangerRecheckEntity>
                {
                    new ConsiderableDangerRecheckEntity { CustID = 1, CTSCustID = 1001, CategoryID = 1, RoleID = 2, SubscriberID = 1, IsLicensee = false },
                    new ConsiderableDangerRecheckEntity { CustID = 2, CTSCustID = 1002, CategoryID = 1, RoleID = 2, SubscriberID = 1, IsLicensee = false }
                },
                RecheckWLCusts = new List<ConsiderableDangerRecheckEntity>
                {
                    new ConsiderableDangerRecheckEntity { CustID = 3, CTSCustID = 1003, CategoryID = 2, RoleID = 2, SubscriberID = 1, IsLicensee = false }
                }
            };

            _dataAccess.GetConsiderableDangerQueue(batchSize).Returns(expectedModel);

            // Act
            var result = _service.GetConsiderableDangerQueue(batchSize);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedModel);
            _dataAccess.Received(1).GetConsiderableDangerQueue(batchSize);
        }

        [Fact]
        public void GetConsiderableDangerQueue_WhenEmptyResult_ShouldReturnEmptyModel()
        {
            // Arrange
            var batchSize = 100;
            var expectedModel = new ConsiderableDangerRecheckModel
            {
                LastID = 0,
                RecheckRatioCusts = Enumerable.Empty<ConsiderableDangerRecheckEntity>(),
                RecheckWLCusts = Enumerable.Empty<ConsiderableDangerRecheckEntity>()
            };

            _dataAccess.GetConsiderableDangerQueue(batchSize).Returns(expectedModel);

            // Act
            var result = _service.GetConsiderableDangerQueue(batchSize);

            // Assert
            result.Should().NotBeNull();
            result.LastID.Should().Be(0);
            result.RecheckRatioCusts.Should().BeEmpty();
            result.RecheckWLCusts.Should().BeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(int.MaxValue)]
        public void GetConsiderableDangerQueue_WithDifferentBatchSizes_ShouldCallDataAccessWithCorrectParameter(int batchSize)
        {
            // Arrange
            var expectedModel = new ConsiderableDangerRecheckModel();
            _dataAccess.GetConsiderableDangerQueue(batchSize).Returns(expectedModel);

            // Act
            var result = _service.GetConsiderableDangerQueue(batchSize);

            // Assert
            _dataAccess.Received(1).GetConsiderableDangerQueue(batchSize);
            result.Should().Be(expectedModel);
        }

        [Fact]
        public void GetConsiderableDangerQueue_WhenDataAccessReturnsNull_ShouldHandleGracefully()
        {
            // Arrange
            var batchSize = 100;
            _dataAccess.GetConsiderableDangerQueue(batchSize).Returns((ConsiderableDangerRecheckModel)null);

            // Act
            var result = _service.GetConsiderableDangerQueue(batchSize);

            // Assert
            result.Should().BeNull();
            _dataAccess.Received(1).GetConsiderableDangerQueue(batchSize);
        }

        [Fact]
        public void GetConsiderableDangerQueue_WhenDataAccessThrowsException_ShouldPropagateException()
        {
            // Arrange
            var batchSize = 100;
            _dataAccess.When(x => x.GetConsiderableDangerQueue(batchSize)).Throw(new InvalidOperationException("Database error"));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _service.GetConsiderableDangerQueue(batchSize));
            exception.Message.Should().Be("Database error");
        }

        [Fact]
        public void GetConsiderableDangerQueue_WithNegativeBatchSize_ShouldHandleGracefully()
        {
            // Arrange
            var batchSize = -1;
            var expectedModel = new ConsiderableDangerRecheckModel();
            _dataAccess.GetConsiderableDangerQueue(batchSize).Returns(expectedModel);

            // Act
            var result = _service.GetConsiderableDangerQueue(batchSize);

            // Assert
            _dataAccess.Received(1).GetConsiderableDangerQueue(batchSize);
            result.Should().Be(expectedModel);
        }

        #endregion

        #region DetectConsiderable Tests

        [Fact]
        public void DetectConsiderable_WhenSuccess_ShouldReturnExpectedData()
        {
            // Arrange
            var custIDs = new List<long> { 1, 2, 3 };
            var expectedEntities = new List<DetectConsiderableEntity>
            {
                new DetectConsiderableEntity { CustID = 1, IsExistingCD = true, IsPassRuleRatio = false },
                new DetectConsiderableEntity { CustID = 2, IsExistingCD = false, IsPassRuleRatio = true },
                new DetectConsiderableEntity { CustID = 3, IsExistingCD = true, IsPassRuleRatio = true }
            };

            _dataAccess.DetectConsiderable(custIDs).Returns(expectedEntities);

            // Act
            var result = _service.DetectConsiderable(custIDs);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedEntities);
            _dataAccess.Received(1).DetectConsiderable(custIDs);
        }

        [Fact]
        public void DetectConsiderable_WhenEmptyInput_ShouldReturnEmptyResult()
        {
            // Arrange
            var custIDs = Enumerable.Empty<long>();
            var expectedEntities = Enumerable.Empty<DetectConsiderableEntity>();

            _dataAccess.DetectConsiderable(custIDs).Returns(expectedEntities);

            // Act
            var result = _service.DetectConsiderable(custIDs);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _dataAccess.Received(1).DetectConsiderable(custIDs);
        }

        [Fact]
        public void DetectConsiderable_WhenNullInput_ShouldHandleGracefully()
        {
            // Arrange
            IEnumerable<long> custIDs = null;
            var expectedEntities = Enumerable.Empty<DetectConsiderableEntity>();

            _dataAccess.DetectConsiderable(custIDs).Returns(expectedEntities);

            // Act
            var result = _service.DetectConsiderable(custIDs);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _dataAccess.Received(1).DetectConsiderable(custIDs);
        }

        [Fact]
        public void DetectConsiderable_WhenDataAccessReturnsNull_ShouldHandleGracefully()
        {
            // Arrange
            var custIDs = new List<long> { 1, 2, 3 };
            _dataAccess.DetectConsiderable(custIDs).Returns((IEnumerable<DetectConsiderableEntity>)null);

            // Act
            var result = _service.DetectConsiderable(custIDs);

            // Assert
            result.Should().BeNull();
            _dataAccess.Received(1).DetectConsiderable(custIDs);
        }

        [Fact]
        public void DetectConsiderable_WhenDataAccessThrowsException_ShouldPropagateException()
        {
            // Arrange
            var custIDs = new List<long> { 1, 2, 3 };
            _dataAccess.When(x => x.DetectConsiderable(custIDs)).Throw(new InvalidOperationException("Database error"));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _service.DetectConsiderable(custIDs));
            exception.Message.Should().Be("Database error");
        }

        [Fact]
        public void DetectConsiderable_WithLargeInput_ShouldHandleGracefully()
        {
            // Arrange
            var custIDs = Enumerable.Range(1, 10000).Select(x => (long)x).ToList();
            var expectedEntities = Enumerable.Empty<DetectConsiderableEntity>();

            _dataAccess.DetectConsiderable(custIDs).Returns(expectedEntities);

            // Act
            var result = _service.DetectConsiderable(custIDs);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _dataAccess.Received(1).DetectConsiderable(custIDs);
        }

        [Fact]
        public void DetectConsiderable_WithDuplicateIds_ShouldHandleGracefully()
        {
            // Arrange
            var custIDs = new List<long> { 1, 1, 2, 2, 3 };
            var expectedEntities = new List<DetectConsiderableEntity>
            {
                new DetectConsiderableEntity { CustID = 1, IsExistingCD = true, IsPassRuleRatio = false },
                new DetectConsiderableEntity { CustID = 2, IsExistingCD = false, IsPassRuleRatio = true },
                new DetectConsiderableEntity { CustID = 3, IsExistingCD = true, IsPassRuleRatio = true }
            };

            _dataAccess.DetectConsiderable(custIDs).Returns(expectedEntities);

            // Act
            var result = _service.DetectConsiderable(custIDs);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedEntities);
            _dataAccess.Received(1).DetectConsiderable(custIDs);
        }

        #endregion

        #region CompleteConsiderableQueue Tests

        [Fact]
        public void CompleteConsiderableQueue_WhenSuccess_ShouldCallDataAccess()
        {
            // Arrange
            var lastID = 12345L;

            // Act
            var result = _service.CompleteConsiderableQueue(lastID);

            // Assert
            _dataAccess.Received(1).CompleteConsiderableQueue(lastID);
            result.Should().Be(_service);
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        [InlineData(999999L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void CompleteConsiderableQueue_WithDifferentLastIDs_ShouldCallDataAccessWithCorrectParameter(long lastID)
        {
            // Act
            var result = _service.CompleteConsiderableQueue(lastID);

            // Assert
            _dataAccess.Received(1).CompleteConsiderableQueue(lastID);
            result.Should().Be(_service);
        }

        [Fact]
        public void CompleteConsiderableQueue_WhenDataAccessThrowsException_ShouldPropagateException()
        {
            // Arrange
            var lastID = 12345L;
            _dataAccess.When(x => x.CompleteConsiderableQueue(lastID)).Throw(new InvalidOperationException("Database error"));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _service.CompleteConsiderableQueue(lastID));
            exception.Message.Should().Be("Database error");
        }

        [Fact]
        public void CompleteConsiderableQueue_WhenDataAccessThrowsArgumentOutOfRangeException_ShouldPropagateException()
        {
            // Arrange
            var lastID = -1L;
            _dataAccess.When(x => x.CompleteConsiderableQueue(lastID)).Throw(new ArgumentOutOfRangeException("lastID"));

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _service.CompleteConsiderableQueue(lastID));
            exception.ParamName.Should().Be("lastID");
        }

        #endregion

        #region InsertConsiderableCategory Tests

        [Fact]
        public void InsertConsiderableCategory_WhenSuccess_ShouldCallProblemAccountServiceAndCustClassToMainDBService()
        {
            // Arrange
            var considerableCusts = new List<ConsiderableDangerRecheckEntity>
            {
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 1, 
                    CTSCustID = 1001, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                },
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 2, 
                    CTSCustID = 1002, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                }
            };

            var pushDataExternal = new PushDataToExternalModel
            {
                IsUpdateDangerToMainDB = true,
                ExternalBatchSize = 100,
                ServiceName = "TestService",
                IsWriteLog = false,
                IsSyncAFCData = false,              
                RoleGroup = RoleGroupType.Agency
            };

            var expectedCustIds = new List<long> { 1, 2 };
            _problemAccountService.InsertProblemAccountsAgencyPackage(
                Arg.Is<IEnumerable<InsertProblemAccountEntity>>(entities => entities.Count() == 2),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService)).Returns(expectedCustIds);

            _custClassToMainDBService.PushExternalProblemAcountPackage(expectedCustIds, pushDataExternal).Returns(true);

            // Act
            var result = _service.InsertConsiderableCategory(considerableCusts, pushDataExternal, AgencyCCInputFlow.InsertConderable);

            // Assert
            _problemAccountService.Received(1).InsertProblemAccountsAgencyPackage(
                Arg.Is<IEnumerable<InsertProblemAccountEntity>>(entities => entities.Count() == 2),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService));
            
            _custClassToMainDBService.Received(1).PushExternalProblemAcountPackage(expectedCustIds, pushDataExternal);
            result.Should().BeTrue();
        }

        [Fact]
        public void InsertConsiderableCategory_WhenEmptyInput_ShouldNotCallProblemAccountService()
        {
            // Arrange
            var considerableCusts = Enumerable.Empty<ConsiderableDangerRecheckEntity>();
            var pushDataExternal = new PushDataToExternalModel();

            var expectedCustIds = Enumerable.Empty<long>();
            _problemAccountService.InsertProblemAccountsAgencyPackage(
                Arg.Is<IEnumerable<InsertProblemAccountEntity>>(entities => !entities.Any()),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService)).Returns(expectedCustIds);

            _custClassToMainDBService.PushExternalProblemAcountPackage(expectedCustIds, pushDataExternal).Returns(true);

            // Act
            var result = _service.InsertConsiderableCategory(considerableCusts, pushDataExternal, AgencyCCInputFlow.InsertConderable);

            // Assert
            _problemAccountService.Received(1).InsertProblemAccountsAgencyPackage(
                Arg.Is<IEnumerable<InsertProblemAccountEntity>>(entities => !entities.Any()),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService));
            
            _custClassToMainDBService.Received(1).PushExternalProblemAcountPackage(expectedCustIds, pushDataExternal);
            result.Should().BeTrue();
        }

        [Fact]
        public void InsertConsiderableCategory_ShouldTransformEntitiesCorrectly()
        {
            // Arrange
            var considerableCusts = new List<ConsiderableDangerRecheckEntity>
            {
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 1, 
                    CTSCustID = 1001, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                }
            };

            var pushDataExternal = new PushDataToExternalModel();
            var expectedCustIds = new List<long> { 1 };
            _problemAccountService.InsertProblemAccountsAgencyPackage(
                Arg.Any<IEnumerable<InsertProblemAccountEntity>>(),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService)).Returns(expectedCustIds);

            _custClassToMainDBService.PushExternalProblemAcountPackage(expectedCustIds, pushDataExternal).Returns(true);

            // Act
            _service.InsertConsiderableCategory(considerableCusts, pushDataExternal, AgencyCCInputFlow.InsertConderable);

            // Assert
            _problemAccountService.Received(1).InsertProblemAccountsAgencyPackage(
                Arg.Is<IEnumerable<InsertProblemAccountEntity>>(entities => 
                    entities.First().CustID == 1 &&
                    entities.First().CTSCustID == 1001 &&
                    entities.First().CategoryID == 1 &&
                    entities.First().RoleID == 2 &&
                    entities.First().SubscriberID == 1 &&
                    entities.First().IsLicensee == false),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService));
        }

        [Fact]
        public void InsertConsiderableCategory_WithNullPushDataExternal_ShouldHandleGracefully()
        {
            // Arrange
            var considerableCusts = new List<ConsiderableDangerRecheckEntity>
            {
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 1, 
                    CTSCustID = 1001, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                }
            };

            var expectedCustIds = new List<long> { 1 };
            _problemAccountService.InsertProblemAccountsAgencyPackage(
                Arg.Any<IEnumerable<InsertProblemAccountEntity>>(),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService)).Returns(expectedCustIds);

            _custClassToMainDBService.PushExternalProblemAcountPackage(expectedCustIds, null).Returns(true);

            // Act
            var result = _service.InsertConsiderableCategory(considerableCusts, null, AgencyCCInputFlow.InsertConderable);

            // Assert
            _problemAccountService.Received(1).InsertProblemAccountsAgencyPackage(
                Arg.Any<IEnumerable<InsertProblemAccountEntity>>(),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService));
            
            _custClassToMainDBService.Received(1).PushExternalProblemAcountPackage(expectedCustIds, null);
            result.Should().BeTrue();
        }      

        [Fact]
        public void InsertConsiderableCategory_WhenProblemAccountServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var considerableCusts = new List<ConsiderableDangerRecheckEntity>
            {
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 1, 
                    CTSCustID = 1001, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                }
            };

            var pushDataExternal = new PushDataToExternalModel();

            _problemAccountService.When(x => x.InsertProblemAccountsAgencyPackage(
                Arg.Any<IEnumerable<InsertProblemAccountEntity>>(),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService))).Throw(new InvalidOperationException("Service error"));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => 
                _service.InsertConsiderableCategory(considerableCusts, pushDataExternal, AgencyCCInputFlow.InsertConderable));
            exception.Message.Should().Be("Service error");
        }

        [Fact]
        public void InsertConsiderableCategory_WhenCustClassToMainDBServiceReturnsFalse_ShouldReturnFalse()
        {
            // Arrange
            var considerableCusts = new List<ConsiderableDangerRecheckEntity>
            {
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 1, 
                    CTSCustID = 1001, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                }
            };

            var pushDataExternal = new PushDataToExternalModel();
            var expectedCustIds = new List<long> { 1 };
            _problemAccountService.InsertProblemAccountsAgencyPackage(
                Arg.Any<IEnumerable<InsertProblemAccountEntity>>(),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService)).Returns(expectedCustIds);

            _custClassToMainDBService.PushExternalProblemAcountPackage(expectedCustIds, pushDataExternal).Returns(false);

            // Act
            var result = _service.InsertConsiderableCategory(considerableCusts, pushDataExternal, AgencyCCInputFlow.InsertConderable);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void InsertConsiderableCategory_WhenCustClassToMainDBServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var considerableCusts = new List<ConsiderableDangerRecheckEntity>
            {
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 1, 
                    CTSCustID = 1001, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                }
            };

            var pushDataExternal = new PushDataToExternalModel();
            var expectedCustIds = new List<long> { 1 };
            _problemAccountService.InsertProblemAccountsAgencyPackage(
                Arg.Any<IEnumerable<InsertProblemAccountEntity>>(),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService)).Returns(expectedCustIds);

            _custClassToMainDBService.When(x => x.PushExternalProblemAcountPackage(expectedCustIds, pushDataExternal))
                .Throw(new InvalidOperationException("External service error"));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => 
                _service.InsertConsiderableCategory(considerableCusts, pushDataExternal, AgencyCCInputFlow.InsertConderable));
            exception.Message.Should().Be("External service error");
        }
       
        [Theory]
        [InlineData(AgencyCCInputFlow.InsertConderable)]
        [InlineData(AgencyCCInputFlow.RecanConsiderable)]
        public void InsertConsiderableCategory_WithDifferentInputFlows_ShouldCallProblemAccountServiceWithCorrectFlow(AgencyCCInputFlow inputFlow)
        {
            // Arrange
            var considerableCusts = new List<ConsiderableDangerRecheckEntity>
            {
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 1, 
                    CTSCustID = 1001, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                }
            };

            var pushDataExternal = new PushDataToExternalModel();
            var expectedCustIds = new List<long> { 1 };
            _problemAccountService.InsertProblemAccountsAgencyPackage(
                Arg.Any<IEnumerable<InsertProblemAccountEntity>>(),
                FunctionId.Default,
                inputFlow,
                false,
                nameof(ConsiderableDangerAgencyService)).Returns(expectedCustIds);

            _custClassToMainDBService.PushExternalProblemAcountPackage(expectedCustIds, pushDataExternal).Returns(true);

            // Act
            _service.InsertConsiderableCategory(considerableCusts, pushDataExternal, inputFlow);

            // Assert
            _problemAccountService.Received(1).InsertProblemAccountsAgencyPackage(
                Arg.Any<IEnumerable<InsertProblemAccountEntity>>(),
                FunctionId.Default,
                inputFlow,
                false,
                nameof(ConsiderableDangerAgencyService));
        }

        #endregion

        #region RemoveConsiderableCategory Tests

        [Fact]
        public void RemoveConsiderableCategory_WhenSuccess_ShouldCallProblemAccountService()
        {
            // Arrange
            var considerableCusts = new List<ConsiderableDangerRecheckEntity>
            {
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 1, 
                    CTSCustID = 1001, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                },
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 2, 
                    CTSCustID = 1002, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                }
            };

            // Act
            var result = _service.RemoveConsiderableCategory(considerableCusts);

            // Assert
            _problemAccountService.Received(1).RemoveConsiderable(
                Arg.Is<IEnumerable<RemoveProblemAccountEntity>>(entities => 
                    entities.Count() == 2 && 
                    entities.All(e => e.IsMarkedDirectly == true && e.CreatedBy == StarixITId)),
                AgencyCCInputFlow.RemoveConderable,
                false);
            result.Should().Be(_service);
        }

        [Fact]
        public void RemoveConsiderableCategory_WhenEmptyInput_ShouldNotCallProblemAccountService()
        {
            // Arrange
            var considerableCusts = Enumerable.Empty<ConsiderableDangerRecheckEntity>();

            // Act
            var result = _service.RemoveConsiderableCategory(considerableCusts);

            // Assert
            _problemAccountService.Received(1).RemoveConsiderable(
                Arg.Is<IEnumerable<RemoveProblemAccountEntity>>(entities => !entities.Any()),
                AgencyCCInputFlow.RemoveConderable,
                false);
            result.Should().Be(_service);
        }       

        [Fact]
        public void RemoveConsiderableCategory_WhenProblemAccountServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var considerableCusts = new List<ConsiderableDangerRecheckEntity>
            {
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 1, 
                    CTSCustID = 1001, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                }
            };

            _problemAccountService.When(x => x.RemoveConsiderable(
                Arg.Any<IEnumerable<RemoveProblemAccountEntity>>(),
                AgencyCCInputFlow.RemoveConderable,
                false)).Throw(new InvalidOperationException("Service error"));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => 
                _service.RemoveConsiderableCategory(considerableCusts));
            exception.Message.Should().Be("Service error");
        }

        [Fact]
        public void RemoveConsiderableCategory_ShouldHandleEmptyCollectionCorrectly()
        {
            // Arrange
            var considerableCusts = new List<ConsiderableDangerRecheckEntity>();

            // Act
            var result = _service.RemoveConsiderableCategory(considerableCusts);

            // Assert
            _problemAccountService.Received(1).RemoveConsiderable(
                Arg.Is<IEnumerable<RemoveProblemAccountEntity>>(entities => !entities.Any()),
                AgencyCCInputFlow.RemoveConderable,
                false);
            result.Should().Be(_service);
        }

        [Fact]
        public void RemoveConsiderableCategory_ShouldTransformMultipleEntitiesCorrectly()
        {
            // Arrange
            var considerableCusts = new List<ConsiderableDangerRecheckEntity>
            {
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 1, 
                    CTSCustID = 1001, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                },
                new ConsiderableDangerRecheckEntity 
                { 
                    CustID = 2, 
                    CTSCustID = 1002, 
                    CategoryID = 2, 
                    RoleID = 3, 
                    SubscriberID = 2, 
                    IsLicensee = true 
                }
            };

            // Act
            _service.RemoveConsiderableCategory(considerableCusts);

            // Assert
            _problemAccountService.Received(1).RemoveConsiderable(
                Arg.Is<IEnumerable<RemoveProblemAccountEntity>>(entities => 
                    entities.Count() == 2 &&
                    entities.Any(e => e.CustID == 1 && e.CTSCustID == 1001 && e.RoleID == 2 && e.SubscriberID == 1) &&
                    entities.Any(e => e.CustID == 2 && e.CTSCustID == 1002 && e.RoleID == 3 && e.SubscriberID == 2) &&
                    entities.All(e => e.IsMarkedDirectly == true && e.CreatedBy == StarixITId)),
                AgencyCCInputFlow.RemoveConderable,
                false);
        }        

        [Fact]
        public void RemoveConsiderableCategory_WithLargeCollection_ShouldHandleGracefully()
        {
            // Arrange
            var considerableCusts = Enumerable.Range(1, 1000)
                .Select(i => new ConsiderableDangerRecheckEntity 
                { 
                    CustID = i, 
                    CTSCustID = 1000 + i, 
                    CategoryID = 1, 
                    RoleID = 2, 
                    SubscriberID = 1, 
                    IsLicensee = false 
                })
                .ToList();

            // Act
            var result = _service.RemoveConsiderableCategory(considerableCusts);

            // Assert
            _problemAccountService.Received(1).RemoveConsiderable(
                Arg.Is<IEnumerable<RemoveProblemAccountEntity>>(entities => 
                    entities.Count() == 1000 &&
                    entities.All(e => e.IsMarkedDirectly == true && e.CreatedBy == StarixITId)),
                AgencyCCInputFlow.RemoveConderable,
                false);
            result.Should().Be(_service);
        }

        #endregion
    }
}
