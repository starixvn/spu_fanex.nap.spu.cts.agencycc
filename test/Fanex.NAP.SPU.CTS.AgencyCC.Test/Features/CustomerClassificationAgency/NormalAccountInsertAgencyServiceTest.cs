using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using Xunit;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Features.CustomerClassificationAgency
{
    public class NormalAccountInsertAgencyServiceTest
    {
        private readonly INormalAccountInsertAgencyDataAccess _dataAccss;
        private readonly IBaseCustomerClassificationAgencyDataAccess _baseDataAccess;
        private readonly INormalAccountInsertAgencyService _service;

        public NormalAccountInsertAgencyServiceTest()
        {
            _dataAccss = Substitute.For<INormalAccountInsertAgencyDataAccess>();
            _baseDataAccess = Substitute.For<IBaseCustomerClassificationAgencyDataAccess>();
            _service = new NormalAccountInsertAgencyService(_baseDataAccess, _dataAccss);
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

        [Fact]
        public void InsertNormalAccount_WhenSuccess()
        {
            // Arrange
            var data = new List<InsertNormalAccountAgencyEntity> { new InsertNormalAccountAgencyEntity(new GetNormalAccountAgencyEntity()) };

            // Act
            _baseDataAccess.InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>()).Returns(new ResultInsertCustomerClassificationAgencyModel());

            var result = _service.InsertNormalAccount(data, string.Empty, false);

            // Assert
            _baseDataAccess.Received(1).InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>());

            Assert.NotNull(result);
        }

        [Fact]
        public void InsertNormalAccount_ThrowException()
        {
            // Arrange
            var data = new List<InsertNormalAccountAgencyEntity> { new InsertNormalAccountAgencyEntity(new GetNormalAccountAgencyEntity()) };

            // Act
            _baseDataAccess.InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>()).Throws<Exception>();

            Action action = () => _service.InsertNormalAccount(data, string.Empty, false);

            // Assert
            _baseDataAccess.Received(0).InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>());

            Assert.Throws<Exception>(action);
        }
    }
}
