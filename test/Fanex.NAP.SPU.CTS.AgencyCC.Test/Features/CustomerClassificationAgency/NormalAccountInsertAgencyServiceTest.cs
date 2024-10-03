using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency;
using NSubstitute;
using Xunit;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Test.Features.CustomerClassificationAgency
{
    public class NormalAccountInsertAgencyServiceTest
    {
        private readonly INormalAccountInsertAgencyDataAccess _dataAccss;
        private readonly INormalAccountInsertAgencyService _service;

        public NormalAccountInsertAgencyServiceTest()
        {
            _dataAccss = Substitute.For<INormalAccountInsertAgencyDataAccess>();
            _service = new NormalAccountInsertAgencyService(Substitute.For<IBaseCustomerClassificationAgencyDataAccess>(), null, _dataAccss);
        }

        [Fact]
        public void GetNormalAccounts_WhenSuccess()
        {
            // Arrange

            // Act
            _service.GetNormalAccounts();

            // Assert
            _dataAccss.Received(1).GetNormalAccount();
        }

        [Fact]
        public void UpdateCompleteNormalAccountInsert_WhenSuccess_ReturnNotException()
        {
            // Arrange
            long scannedMaxId = 10;

            // Act
            _service.CompleteNormalAccountInsert(scannedMaxId, true);

            // Assert
            _dataAccss.Received(1).CompleteNormalAccountInsert(scannedMaxId);
        }
    }
}
