namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Features.CustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.DailyConsiderableDangerAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
    using FluentAssertions;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public class ConsiderableDangerAgencyServiceTest
    {
        private readonly IConsiderableDangerAgencyDataAccess _dataAccess;
        private readonly IConsiderableDangerAgencyService _service;
        private readonly IProblemAccountAgencyService _problemAccountService;

        public ConsiderableDangerAgencyServiceTest()
        {
            _dataAccess = Substitute.For<IConsiderableDangerAgencyDataAccess>();
            _problemAccountService = Substitute.For<IProblemAccountAgencyService>();
            _service = new ConsiderableDangerAgencyService(_dataAccess, _problemAccountService);
        }

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

        [Fact]
        public void DetectConsiderable_WhenSuccess_ShouldReturnExpectedData()
        {
            // Arrange
            var custIDs = new List<long> { 1, 2, 3 };
            var expectedEntities = new List<DetectConsiderableEntity>
            {
                new DetectConsiderableEntity { CustID = 1, IsPassRuleConsiderable = true, IsPassRuleRatio = false },
                new DetectConsiderableEntity { CustID = 2, IsPassRuleConsiderable = false, IsPassRuleRatio = true },
                new DetectConsiderableEntity { CustID = 3, IsPassRuleConsiderable = true, IsPassRuleRatio = true }
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

        [Fact]
        public void InsertConsiderableCategory_WhenSuccess_ShouldCallProblemAccountService()
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
            var result = _service.InsertConsiderableCategory(considerableCusts);

            // Assert
            _problemAccountService.Received(1).InsertProblemAccountsAgencyPackage(
                Arg.Is<IEnumerable<InsertProblemAccountEntity>>(entities => entities.Count() == 2),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService));
            result.Should().Be(_service);
        }

        [Fact]
        public void InsertConsiderableCategory_WhenEmptyInput_ShouldNotCallProblemAccountService()
        {
            // Arrange
            var considerableCusts = Enumerable.Empty<ConsiderableDangerRecheckEntity>();

            // Act
            var result = _service.InsertConsiderableCategory(considerableCusts);

            // Assert
            _problemAccountService.Received(1).InsertProblemAccountsAgencyPackage(
                Arg.Is<IEnumerable<InsertProblemAccountEntity>>(entities => !entities.Any()),
                FunctionId.Default,
                AgencyCCInputFlow.InsertConderable,
                false,
                nameof(ConsiderableDangerAgencyService));
            result.Should().Be(_service);
        }

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
            _problemAccountService.Received(1).RemoveProblemAccount(
                Arg.Is<IEnumerable<RemoveProblemAccountEntity>>(entities => 
                    entities.Count() == 2 && 
                    entities.All(e => e.IsMarkedDirectly == true && e.CreatedBy == StarixITId)),
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
            _problemAccountService.Received(1).RemoveProblemAccount(
                Arg.Is<IEnumerable<RemoveProblemAccountEntity>>(entities => !entities.Any()),
                false);
            result.Should().Be(_service);
        }

        [Fact]
        public void RemoveConsiderableCategory_ShouldSetCorrectProperties()
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

            // Act
            _service.RemoveConsiderableCategory(considerableCusts);

            // Assert
            _problemAccountService.Received(1).RemoveProblemAccount(
                Arg.Is<IEnumerable<RemoveProblemAccountEntity>>(entities => 
                    entities.First().CustID == 1 &&
                    entities.First().CTSCustID == 1001 &&
                    entities.First().RoleID == 2 &&
                    entities.First().SubscriberID == 1 &&
                    entities.First().IsMarkedDirectly == true &&
                    entities.First().CreatedBy == StarixITId),
                false);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
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

        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        [InlineData(999999L)]
        public void CompleteConsiderableQueue_WithDifferentLastIDs_ShouldCallDataAccessWithCorrectParameter(long lastID)
        {
            // Act
            var result = _service.CompleteConsiderableQueue(lastID);

            // Assert
            _dataAccess.Received(1).CompleteConsiderableQueue(lastID);
            result.Should().Be(_service);
        }
    }
}
