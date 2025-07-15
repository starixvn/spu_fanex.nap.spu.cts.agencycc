using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
using Fanex.NAP.SPU.CTS.AgencyCC.Shares.Models;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using Xunit;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;
using static Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Consts.CustomerClassificationAgency.ProblemAccountAgencyServiceConsts;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Consts.CustomerClassificationAgency
{
    public class ProblemAccountAgencyServiceTest
    {
        private readonly IBaseCustomerClassificationAgencyDataAccess _baseCCAgencyDataAccess;
        private readonly IProblemAccountAgencyDataAccess _paAgencyDataAccess;

        private readonly IProblemAccountAgencyService _service;

        public ProblemAccountAgencyServiceTest()
        {
            _baseCCAgencyDataAccess = Substitute.For<IBaseCustomerClassificationAgencyDataAccess>();
            _paAgencyDataAccess = Substitute.For<IProblemAccountAgencyDataAccess>();


            _service = new ProblemAccountAgencyService(_baseCCAgencyDataAccess, _paAgencyDataAccess);
        }

        [Fact]
        public void RemoveProblemAccount_WhenSuccess()
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

            _baseCCAgencyDataAccess.UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>()).Returns(new DBResult());

            var result = _service.RemoveProblemAccount(new List<RemoveProblemAccountEntity> { new RemoveProblemAccountEntity() }, true);

            // Assert
            _baseCCAgencyDataAccess.Received(1).RemoveCustomerClassificationAgency(Arg.Any<AgencyCCInputFlow>(), Arg.Any<RemoveCustomerClassificationAgencyModel>());

            Assert.NotNull(result);
        }

        [Fact]
        public void RemoveProblemAccount_ThrowException()
        {
            // Arrange

            // Act
            _baseCCAgencyDataAccess.RemoveCustomerClassificationAgency(Arg.Any<AgencyCCInputFlow>(), Arg.Any<RemoveCustomerClassificationAgencyModel>())
                .Throws<Exception>();

            Action action = () => _service.RemoveProblemAccount(new List<RemoveProblemAccountEntity> { new RemoveProblemAccountEntity() }, true);

            // Assert
            _baseCCAgencyDataAccess.Received(0).RemoveCustomerClassificationAgency(Arg.Any<AgencyCCInputFlow>(), Arg.Any<RemoveCustomerClassificationAgencyModel>());

            Assert.Throws<Exception>(action);
        }

#pragma warning disable S125
        // not handle SMA
        //[Fact]
        //public void RemovePALogics_WhenSuccess()
        //{
        //    // Arrange
        //    var removePALogicsMethod = TestExtension.GetPrivateMethod(typeof(ProblemAccountAgencyService), PrivateMethodName.RemovePALogics);

        //    // Act
        //    _baseCCAgencyDataAccess.UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>()).Returns(new DBResult());

        //    removePALogicsMethod.Invoke(_service, new object[] { new List<ResultRemoveCustomerClassificationEntity> { new ResultRemoveCustomerClassificationEntity { IsCleanPA = true } } });

        //    // Assert
        //    _baseCCAgencyDataAccess.Received(1).UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>());
        //}

        //[Fact]
        //public void RemovePALogics_ThrowException()
        //{
        //    // Arrange
        //    var removePALogicsMethod = TestExtension.GetPrivateMethod(typeof(ProblemAccountAgencyService), PrivateMethodName.RemovePALogics);

        //    // Act
        //    _baseCCAgencyDataAccess.UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>()).Throws<Exception>();

        //    Action action = () => removePALogicsMethod.Invoke(
        //        _service, new object[] { new List<ResultRemoveCustomerClassificationEntity> { new ResultRemoveCustomerClassificationEntity { IsCleanPA = true } } });

        //    // Assert
        //    _baseCCAgencyDataAccess.Received(0).UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>());

        //    Assert.Throws<TargetInvocationException>(action);
        //}

        //[Fact]
        //public void UpdateBetLimitOfFlaggedCustomer_WhenSuccess()
        //{
        //    // Arrange
        //    var removePALogicsMethod = TestExtension.GetPrivateMethod(typeof(ProblemAccountAgencyService), PrivateMethodName.UpdateBetLimitOfFlaggedCustomer);

        //    // Act
        //    _baseCCAgencyDataAccess.UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>()).Returns(new DBResult());

        //    removePALogicsMethod.Invoke(_service, new object[] { new List<long> { 1, 2, 3, 4 } });

        //    // Assert
        //    _baseCCAgencyDataAccess.Received(1).UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>());
        //}

        //[Fact]
        //public void UpdateBetLimitOfFlaggedCustomer_ThrowException()
        //{
        //    // Arrange
        //    var removePALogicsMethod = TestExtension.GetPrivateMethod(typeof(ProblemAccountAgencyService), PrivateMethodName.UpdateBetLimitOfFlaggedCustomer);

        //    // Act
        //    _baseCCAgencyDataAccess.UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>()).Throws<Exception>();

        //    Action action = () => removePALogicsMethod.Invoke(_service, new object[] { new List<long> { 1, 2, 3, 4 } });

        //    // Assert
        //    _baseCCAgencyDataAccess.Received(0).UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>());

        //    Assert.Throws<TargetInvocationException>(action);
        //}

        //[Fact]
        //public void UpdateBetLimitOfUnflaggedCustomer_WhenSuccess()
        //{
        //    // Arrange
        //    var removePALogicsMethod = TestExtension.GetPrivateMethod(typeof(ProblemAccountAgencyService), PrivateMethodName.UpdateBetLimitOfUnflaggedCustomer);

        //    // Act
        //    _baseCCAgencyDataAccess.UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>()).Returns(new DBResult());

        //    removePALogicsMethod.Invoke(_service, new object[] { new List<long> { 1, 2, 3, 4 } });

        //    // Assert
        //    _baseCCAgencyDataAccess.Received(1).UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>());
        //}

        //[Fact]
        //public void UpdateBetLimitOfUnflaggedCustomer_ThrowException()
        //{
        //    // Arrange
        //    var removePALogicsMethod = TestExtension.GetPrivateMethod(typeof(ProblemAccountAgencyService), PrivateMethodName.UpdateBetLimitOfUnflaggedCustomer);

        //    // Act
        //    _baseCCAgencyDataAccess.UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>()).Throws<Exception>();

        //    Action action = () => removePALogicsMethod.Invoke(_service, new object[] { new List<long> { 1, 2, 3, 4 } });

        //    // Assert
        //    _baseCCAgencyDataAccess.Received(0).UpdateBetLimitOfUnflaggedCustomer(Arg.Any<string>());

        //    Assert.Throws<TargetInvocationException>(action);
        //}
#pragma warning disable S125
    }
}
