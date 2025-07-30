namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Features.ArchiveData.ClassificationHistory
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.ArchiveData.ClassificationHistory;
    using FluentAssertions;
    using NSubstitute;
    using System.Collections.Generic;
    using Xunit;

    public class ArchiveHistoryClassificationServiceTest
    {
        private readonly IArchiveHistoryClassificationDataAccess _dataAccess;
        private readonly IArchiveHistoryClassificationService _service;

        public ArchiveHistoryClassificationServiceTest()
        {
            _dataAccess = Substitute.For<IArchiveHistoryClassificationDataAccess>();
            _service = new ArchiveHistoryClassificationService(_dataAccess);
        }

        [Fact]
        public void GetHistoryClassifyArchive_WhenSuccess_ReturnData()
        {
            // Arrange
            var expectedData = new List<HistoryClassifyArchiveEntity>
            {
                new HistoryClassifyArchiveEntity { ID = 1, CustID = 100, CTSCustID = 200 },
                new HistoryClassifyArchiveEntity { ID = 2, CustID = 101, CTSCustID = 201 },
                new HistoryClassifyArchiveEntity { ID = 3, CustID = 102, CTSCustID = 202 }
            };

            _dataAccess.GetHistoryClassifyArchive().Returns(expectedData);

            // Act
            var result = _service.GetHistoryClassifyArchive();

            // Assert
            result.Should().BeEquivalentTo(expectedData);
            _dataAccess.Received(1).GetHistoryClassifyArchive();
        }

        [Fact]
        public void GetHistoryClassifyArchive_WhenNoData_ReturnEmptyList()
        {
            // Arrange
            var expectedData = new List<HistoryClassifyArchiveEntity>();
            _dataAccess.GetHistoryClassifyArchive().Returns(expectedData);

            // Act
            var result = _service.GetHistoryClassifyArchive();

            // Assert
            result.Should().BeEmpty();
            _dataAccess.Received(1).GetHistoryClassifyArchive();
        }

        [Fact]
        public void InsertHistoryClassifyArchive_WhenValidData_CallsDataAccess()
        {
            // Arrange
            var historyDataJson = "{\"CustID\": 100, \"CTSCustID\": 200}";

            // Act
            _service.InsertHistoryClassifyArchive(historyDataJson);

            // Assert
            _dataAccess.Received(1).InsertHistoryClassifyArchive(historyDataJson);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("{\"test\": \"data\"}")]
        public void InsertHistoryClassifyArchive_WithVariousInputs_CallsDataAccess(string historyDataJson)
        {
            // Act
            _service.InsertHistoryClassifyArchive(historyDataJson);

            // Assert
            _dataAccess.Received(1).InsertHistoryClassifyArchive(historyDataJson);
        }

        [Fact]
        public void CompleteHistoryClassifyArchive_WhenValidArchiveIDs_CallsDataAccess()
        {
            // Arrange
            var archiveIDs = new List<long> { 1, 2, 3, 4, 5 };

            // Act
            _service.CompleteHistoryClassifyArchive(archiveIDs);

            // Assert
            _dataAccess.Received(1).CompleteHistoryClassifyArchive(archiveIDs);
        }

        [Fact]
        public void CompleteHistoryClassifyArchive_WhenEmptyList_CallsDataAccess()
        {
            // Arrange
            var archiveIDs = new List<long>();

            // Act
            _service.CompleteHistoryClassifyArchive(archiveIDs);

            // Assert
            _dataAccess.Received(1).CompleteHistoryClassifyArchive(archiveIDs);
        }

        [Fact]
        public void CompleteHistoryClassifyArchive_WhenSingleArchiveID_CallsDataAccess()
        {
            // Arrange
            var archiveIDs = new List<long> { 1 };

            // Act
            _service.CompleteHistoryClassifyArchive(archiveIDs);

            // Assert
            _dataAccess.Received(1).CompleteHistoryClassifyArchive(archiveIDs);
        }

        [Fact]
        public void CompleteHistoryClassifyArchive_WhenNullArchiveIDs_CallsDataAccess()
        {
            // Arrange
            IEnumerable<long> archiveIDs = null;

            // Act
            _service.CompleteHistoryClassifyArchive(archiveIDs);

            // Assert
            _dataAccess.Received(1).CompleteHistoryClassifyArchive(archiveIDs);
        }
    }
}
