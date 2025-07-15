using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Entities;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Shares.Models;
using Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;
using static Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Consts.CustomerClassificationAgency.RescanCustClassificationAgencyConsts;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Features.CustomerClassificationAgency
{
    public class RescanCustClassificationAgencyServiceTest
    {
        private readonly IRescanCustClassificationAgencyDataAccess _dataAccess;       
        private readonly ICustClassToMainDBService _custClassToMainDBService;
        private readonly IBaseCustomerClassificationAgencyDataAccess _baseCCAgencyDataAccess;
        private readonly IProblemAccountAgencyDataAccess _paAgencyDataAccess;

        private readonly IRescanCustClassificationAgencyService _service;

        public RescanCustClassificationAgencyServiceTest()
        {
            _dataAccess = Substitute.For<IRescanCustClassificationAgencyDataAccess>();
            _custClassToMainDBService = Substitute.For<ICustClassToMainDBService>();
            _baseCCAgencyDataAccess = Substitute.For<IBaseCustomerClassificationAgencyDataAccess>();
            _paAgencyDataAccess = Substitute.For<IProblemAccountAgencyDataAccess>();

            _service = new RescanCustClassificationAgencyService(_baseCCAgencyDataAccess, _paAgencyDataAccess, _dataAccess, _custClassToMainDBService);
        }

        [Fact]
        public void GetAgencyCusts_WhenSuccess()
        {
            // Arrange
            var data = new List<RoleCustomerEntity>
            {
                new RoleCustomerEntity
                {
                    RoleID = 2
                }
            };

            var smaCustIdsField = TestExtension.GetPrivateFieldInfo(typeof(RescanCustClassificationAgencyService), PrivateFieldName.SMACustIds);

            // Act
            _dataAccess.GetRoleCustomer(Arg.Any<string>()).Returns(data);

            _service.GetAgencyCusts(new List<long> { 1, 2, 3, 4 });

            var result = smaCustIdsField.GetValue(_service);

            // Assert
            _dataAccess.Received(1).GetRoleCustomer(Arg.Any<string>());

            Assert.NotNull(result);
        }

        [Fact]
        public void GetAgencyCusts_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.GetRoleCustomer(Arg.Any<string>()).Throws<Exception>();

            Action action = () => _service.GetAgencyCusts(new List<long> { 1, 2, 3, 4 });

            // Assert
            _dataAccess.Received(0).GetRoleCustomer(Arg.Any<string>());

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void PushExternalProblemAccounts_WhenSuccess()
        {
            // Arrange

            // Act
            _custClassToMainDBService.PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>()).Returns(true);

            var result = _service.PushExternalProblemAccounts(new List<long> { 1, 2, 3, 4 }, new PushDataToExternalModel());

            // Assert
            _custClassToMainDBService.Received(1).PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>());

            Assert.True(result);
        }

        [Fact]
        public void PushExternalProblemAccounts_ThrowException()
        {
            // Arrange

            // Act
            _custClassToMainDBService.PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>()).Throws<Exception>();

            Action action = () => _service.PushExternalProblemAccounts(new List<long> { 1, 2, 3, 4 }, new PushDataToExternalModel());

            // Assert
            _custClassToMainDBService.Received(0).PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>());

            Assert.Throws<Exception>(action);
        }

        [Fact]
        public void PreNormalClassification_WhenSuccess()
        {
            // Arrange
            var preNormalClassificationMethod = TestExtension.GetPrivateMethod(typeof(RescanCustClassificationAgencyService), PrivateMethodName.PreNormalClassification);

            // Act
            _dataAccess.PreNormalClassification(Arg.Any<string>(), Arg.Any<FunctionId>(), Arg.Any<RescanNormalType>()).Returns(0);

            preNormalClassificationMethod.Invoke(_service, new object[] { new List<long> { 1, 2, 3, 4, }, new RescanParametersModel() });

            // Assert
            _dataAccess.Received(1).PreNormalClassification(Arg.Any<string>(), Arg.Any<FunctionId>(), Arg.Any<RescanNormalType>());
        }

        [Fact]
        public void PreNormalClassification_ThrowException()
        {
            // Arrange
            var preNormalClassificationMethod = TestExtension.GetPrivateMethod(typeof(RescanCustClassificationAgencyService), PrivateMethodName.PreNormalClassification);

            // Act
            _dataAccess.PreNormalClassification(Arg.Any<string>(), Arg.Any<FunctionId>(), Arg.Any<RescanNormalType>()).Throws<Exception>();

            Action action = () => preNormalClassificationMethod.Invoke(_service, new object[] { new List<long> { 1, 2, 3, 4, }, new RescanParametersModel() });

            // Assert
            _dataAccess.Received(0).PreNormalClassification(Arg.Any<string>(), Arg.Any<FunctionId>(), Arg.Any<RescanNormalType>());

            Assert.Throws<TargetInvocationException>(action);
        }

        [Fact]
        public void RescanProblemAccount_WhenSuccess()
        {
            // Arrange
            var rescanProblemAccountMethod = TestExtension.GetPrivateMethod(typeof(RescanCustClassificationAgencyService), PrivateMethodName.RescanProblemAccount);

            // Act
            _baseCCAgencyDataAccess.InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>())
                .Returns(new ResultInsertCustomerClassificationAgencyModel
                {
                    CustInfos = new List<CustInfoClassificationAgencyEntity>
                    {
                        new CustInfoClassificationAgencyEntity()
                    }
                });

            _baseCCAgencyDataAccess.UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>()).Returns(new DBResult());

            _custClassToMainDBService.PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>()).Returns(true);

            var result = rescanProblemAccountMethod.Invoke(
                _service,
                new object[] {
                    new List<RescanCustomerClassificationAgencyEntity> { 
                        new RescanCustomerClassificationAgencyEntity()
                    },
                    new RescanParametersModel(),
                    AgencyCCInputFlow.InsertPACategory
                });

            // Assert
            _baseCCAgencyDataAccess.Received(1).InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>());
            _custClassToMainDBService.Received(1).PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>());

            Assert.NotNull(result);
        }

        [Fact]
        public void RescanProblemAccount_ThrowException()
        {
            // Arrange
            var rescanProblemAccountMethod = TestExtension.GetPrivateMethod(typeof(RescanCustClassificationAgencyService), PrivateMethodName.RescanProblemAccount);

            // Act
            _baseCCAgencyDataAccess.InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>())
                .Throws<Exception>();

            Action action = () => rescanProblemAccountMethod.Invoke(
                _service,
                new object[] {
                    new List<RescanCustomerClassificationAgencyEntity> {
                        new RescanCustomerClassificationAgencyEntity()
                    },
                    new RescanParametersModel(),
                    AgencyCCInputFlow.InsertPACategory
                });

            // Assert
            _baseCCAgencyDataAccess.Received(0).InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>());
            _baseCCAgencyDataAccess.Received(0).UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>());
            _custClassToMainDBService.Received(0).PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>());

            Assert.Throws<TargetInvocationException>(action);
        }
    }
}
