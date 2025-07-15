using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using Xunit;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Consts.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    public class AffectedDownlineAgencyServiceTest
    {
        private readonly IBaseCustomerClassificationAgencyDataAccess _baseCCAgencyDataAccess;

        private readonly IAffectedDownlineAgencyService _service;

        public AffectedDownlineAgencyServiceTest()
        {
            _baseCCAgencyDataAccess = Substitute.For<IBaseCustomerClassificationAgencyDataAccess>();

            _service = new AffectedDownlineAgencyService(_baseCCAgencyDataAccess);
        }

        [Fact]
        public void InsertVVIP_WhenSuccess()
        {
            // Arrange

            // Act
            _baseCCAgencyDataAccess.InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>())
                .Returns(new ResultInsertCustomerClassificationAgencyModel
                {
                    CustInfos = new List<CustInfoClassificationAgencyEntity>
                    {
                        new CustInfoClassificationAgencyEntity()
                    }
                });

            var result = _service.InsertVVIP(new CustClassificationFromQueueModel
            {
                AgencyQueue = new List<CustClassificationFromQueueEntity> { new CustClassificationFromQueueEntity() }
            }, FunctionId.InsertNewCustJob, AgencyCCInputFlow.InsertVVIP, true);

            // Assert
            _baseCCAgencyDataAccess.Received(1).InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>());

            Assert.NotNull(result);
        }

        [Fact]
        public void InsertVVIP_ThrowException()
        {
            // Arrange

            // Act
            _baseCCAgencyDataAccess.InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>())
                .Throws<Exception>();

            Action action = () => _service.InsertVVIP(new CustClassificationFromQueueModel
            {
                AgencyQueue = new List<CustClassificationFromQueueEntity> { new CustClassificationFromQueueEntity() }
            }, FunctionId.InsertNewCustJob, AgencyCCInputFlow.InsertVVIP, true);

            // Assert
            _baseCCAgencyDataAccess.Received(0).InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>());

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void RemoveVVIP_WhenSuccess()
        {
            // Arrange

            // Act
            _baseCCAgencyDataAccess.RemoveCustomerClassificationAgency(Arg.Any<AgencyCCInputFlow>(), Arg.Any<RemoveCustomerClassificationAgencyModel>())
                .Returns(new ResultRemoveCustomerClassificationAgencyModel
                {
                    CustInfos = new List<ResultRemoveCustomerClassificationEntity>
                    {
                        new ResultRemoveCustomerClassificationEntity()
                    }
                });

            var result = _service.RemoveVVIP(new CustClassificationFromQueueModel
            {
                AgencyQueue = new List<CustClassificationFromQueueEntity> { new CustClassificationFromQueueEntity() }
            });

            // Assert
            _baseCCAgencyDataAccess.Received(1).RemoveCustomerClassificationAgency(Arg.Any<AgencyCCInputFlow>(), Arg.Any<RemoveCustomerClassificationAgencyModel>());

            Assert.NotNull(result);
        }

        [Fact]
        public void RemoveVVIP_ThrowException()
        {
            // Arrange

            // Act
            _baseCCAgencyDataAccess.RemoveCustomerClassificationAgency(Arg.Any<AgencyCCInputFlow>(), Arg.Any<RemoveCustomerClassificationAgencyModel>())
                .Throws<Exception>();

            Action action = () => _service.RemoveVVIP(new CustClassificationFromQueueModel
            {
                AgencyQueue = new List<CustClassificationFromQueueEntity> { new CustClassificationFromQueueEntity() }
            });

            // Assert
            _baseCCAgencyDataAccess.Received(0).RemoveCustomerClassificationAgency(Arg.Any<AgencyCCInputFlow>(), Arg.Any<RemoveCustomerClassificationAgencyModel>());

            Assert.Throws<Exception>(action);
        }
    }
}
