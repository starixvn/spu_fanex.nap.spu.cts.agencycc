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
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Start Tests

        [Fact]
        public void Start_WhenCalled_ShouldInitializeServiceState()
        {
            // Act
            var result = _service.Start();

            // Assert
            result.Should().Be(_service);
            
            var smaCustIdsField = TestExtension.GetPrivateFieldInfo(typeof(RescanCustClassificationAgencyService), PrivateFieldName.SMACustIds);
            var isSuccessField = TestExtension.GetPrivateFieldInfo(typeof(RescanCustClassificationAgencyService), PrivateFieldName.IsSuccess);
            
            var smaCustIds = smaCustIdsField.GetValue(_service);
            var isSuccess = isSuccessField.GetValue(_service);
            
            smaCustIds.Should().NotBeNull();
            ((List<long>)smaCustIds).Should().BeEmpty();
            isSuccess.Should().Be(true);
        }

        [Fact]
        public void Start_WhenCalledMultipleTimes_ShouldResetStateEachTime()
        {
            // Arrange
            var smaCustIdsField = TestExtension.GetPrivateFieldInfo(typeof(RescanCustClassificationAgencyService), PrivateFieldName.SMACustIds);
            var isSuccessField = TestExtension.GetPrivateFieldInfo(typeof(RescanCustClassificationAgencyService), PrivateFieldName.IsSuccess);
            
            // Set some initial state
            smaCustIdsField.SetValue(_service, new List<long> { 1, 2, 3 });
            isSuccessField.SetValue(_service, false);

            // Act
            var result = _service.Start();

            // Assert
            result.Should().Be(_service);
            
            var smaCustIds = smaCustIdsField.GetValue(_service);
            var isSuccess = isSuccessField.GetValue(_service);
            
            ((List<long>)smaCustIds).Should().BeEmpty();
            isSuccess.Should().Be(true);
        }

        #endregion

        #region GetAgencyCusts Tests

        [Fact]
        public void GetAgencyCusts_WhenSuccess_ShouldFilterAgencyCustomers()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3, 4, 5 };
            var data = new List<RoleCustomerEntity>
            {
                new RoleCustomerEntity { CustID = 1, RoleID = (int)CustomerRole.Agent },
                new RoleCustomerEntity { CustID = 2, RoleID = (int)CustomerRole.Master },
                new RoleCustomerEntity { CustID = 3, RoleID = (int)CustomerRole.Super },         
                new RoleCustomerEntity { CustID = 5, RoleID = (int)CustomerRole.Agent }
            };

            _dataAccess.GetRoleCustomer(string.Join(",", custIds)).Returns(data);

            // Act
            var result = _service.GetAgencyCusts(custIds);

            // Assert
            result.Should().Be(_service);
            _dataAccess.Received(1).GetRoleCustomer(string.Join(",", custIds));
            
            var smaCustIdsField = TestExtension.GetPrivateFieldInfo(typeof(RescanCustClassificationAgencyService), PrivateFieldName.SMACustIds);
            var smaCustIds = (List<long>)smaCustIdsField.GetValue(_service);
            
            smaCustIds.Should().HaveCount(4);
            smaCustIds.Should().Contain(1);
            smaCustIds.Should().Contain(2);
            smaCustIds.Should().Contain(3);
            smaCustIds.Should().Contain(5);
        }      
       
        [Fact]
        public void GetAgencyCusts_WhenDataAccessThrowsException_ShouldPropagateException()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3 };
            _dataAccess.GetRoleCustomer(string.Join(",", custIds)).Throws(new InvalidOperationException("Database error"));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _service.GetAgencyCusts(custIds));
            exception.Message.Should().Be("Database error");
        }      

        #endregion

        #region PushExternalProblemAccounts Tests      

        [Fact]
        public void PushExternalProblemAccounts_WhenEmptyInput_ShouldReturnTrue()
        {
            // Arrange
            var custIds = Enumerable.Empty<long>();
            var paras = new PushDataToExternalModel();

            // Act
            var result = _service.PushExternalProblemAccounts(custIds, paras);

            // Assert
            result.Should().BeTrue();
            _custClassToMainDBService.Received(0).PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>());
        }       

        [Fact]
        public void PushExternalProblemAccounts_WithDuplicateIds_ShouldCallWithDistinctIds()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 1, 3, 2, 4 };
            var paras = new PushDataToExternalModel();

            _custClassToMainDBService.PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), paras).Returns(true);

            // Act
            var result = _service.PushExternalProblemAccounts(custIds, paras);

            // Assert
            result.Should().BeTrue();
            _custClassToMainDBService.Received(1).PushExternalProblemAcountPackage(
                Arg.Is<IEnumerable<long>>(ids => ids.Count() == 4 && ids.Contains(1) && ids.Contains(2) && ids.Contains(3) && ids.Contains(4)), 
                paras);
        }

        #endregion

        #region PreNormalClassification Tests

        [Fact]
        public void PreNormalClassification_WhenSuccess_ShouldCallDataAccess()
        {
            // Arrange
            var preNormalClassificationMethod = TestExtension.GetPrivateMethod(typeof(RescanCustClassificationAgencyService), PrivateMethodName.PreNormalClassification);
            var custNormals = new List<long> { 1, 2, 3, 4 };
            var paras = new RescanParametersModel
            {
                FunctionId = FunctionId.Default,
                RescanNormalType = RescanNormalType.Auto_Customer
            };

            _dataAccess.PreNormalClassification(string.Join(",", custNormals), paras.FunctionId, paras.RescanNormalType).Returns(1);

            // Act
            preNormalClassificationMethod.Invoke(_service, new object[] { custNormals, paras });

            // Assert
            _dataAccess.Received(1).PreNormalClassification(string.Join(",", custNormals), paras.FunctionId, paras.RescanNormalType);
        }

        [Fact]
        public void PreNormalClassification_WhenDataAccessThrowsException_ShouldPropagateException()
        {
            // Arrange
            var preNormalClassificationMethod = TestExtension.GetPrivateMethod(typeof(RescanCustClassificationAgencyService), PrivateMethodName.PreNormalClassification);
            var custNormals = new List<long> { 1, 2, 3 };
            var paras = new RescanParametersModel
            {
                FunctionId = FunctionId.Default,
                RescanNormalType = RescanNormalType.Auto_InactiveCustomer
            };

            _dataAccess.PreNormalClassification(string.Join(",", custNormals), paras.FunctionId, paras.RescanNormalType).Throws(new InvalidOperationException("Database error"));

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() => 
                preNormalClassificationMethod.Invoke(_service, new object[] { custNormals, paras }));
            exception.InnerException.Should().BeOfType<InvalidOperationException>();
            exception.InnerException.Message.Should().Be("Database error");
        }

        [Fact]
        public void PreNormalClassification_WithEmptyInput_ShouldHandleGracefully()
        {
            // Arrange
            var preNormalClassificationMethod = TestExtension.GetPrivateMethod(typeof(RescanCustClassificationAgencyService), PrivateMethodName.PreNormalClassification);
            var custNormals = Enumerable.Empty<long>();
            var paras = new RescanParametersModel
            {
                FunctionId = FunctionId.Default,
                RescanNormalType = RescanNormalType.Auto_Customer
            };

            _dataAccess.PreNormalClassification("", paras.FunctionId, paras.RescanNormalType).Returns(0);

            // Act
            preNormalClassificationMethod.Invoke(_service, new object[] { custNormals, paras });

            // Assert
            _dataAccess.Received(1).PreNormalClassification("", paras.FunctionId, paras.RescanNormalType);
        }

        #endregion

        #region RescanProblemAccount Tests
       
        [Fact]
        public void RescanProblemAccount_WhenBaseServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            var rescanProblemAccountMethod = TestExtension.GetPrivateMethod(typeof(RescanCustClassificationAgencyService), PrivateMethodName.RescanProblemAccount);
            var custProblems = new List<RescanCustomerClassificationAgencyEntity>
            {
                new RescanCustomerClassificationAgencyEntity { CustID = 1, CTSCustID = 1001, CategoryID = 1, RoleID = 2, SubscriberID = 1, IsLicensee = false }
            };
            var paras = new RescanParametersModel
            {
                FunctionId = FunctionId.Default,
                IsToQueue = false,
                ServiceName = "TestService",
                PushDataExternal = new PushDataToExternalModel()
            };
            var ccInputFlow = AgencyCCInputFlow.InsertPACategory;

            _baseCCAgencyDataAccess.InsertCustomerClassification(ccInputFlow, Arg.Any<InsertCustomerClassificationAgencyModel>()).Throws(new InvalidOperationException("Service error"));

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() => 
                rescanProblemAccountMethod.Invoke(_service, new object[] { custProblems, paras, ccInputFlow }));
            exception.InnerException.Should().BeOfType<InvalidOperationException>();
            exception.InnerException.Message.Should().Be("Service error");
        }

        #endregion

        #region RescanCustomerClassificationAgency Tests

        [Fact]
        public void RescanCustomerClassificationAgency_WhenSuccess_ShouldReturnTrue()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3 };
            var paras = new RescanParametersModel
            {
                FunctionId = FunctionId.Default,
                IsToQueue = false,
                ServiceName = "TestService",
                PushDataExternal = new PushDataToExternalModel()
            };

            var custRescans = new GetRescanCustomerClassificationAgencyModel
            {
                CustInfos = new List<RescanCustomerClassificationAgencyEntity>
                {
                    new RescanCustomerClassificationAgencyEntity { CustID = 1, InputFlowID = (int)AgencyCCInputFlow.RescanNormal },
                    new RescanCustomerClassificationAgencyEntity { CustID = 2, InputFlowID = (int)AgencyCCInputFlow.InsertPACategory }
                }
            };

            var roleCustomers = new List<RoleCustomerEntity>
            {
                new RoleCustomerEntity { CustID = 1, RoleID = (int)CustomerRole.Agent },
                new RoleCustomerEntity { CustID = 2, RoleID = (int)CustomerRole.Master },
                new RoleCustomerEntity { CustID = 3, RoleID = (int)CustomerRole.Agent }
            };

            _dataAccess.GetRoleCustomer(string.Join(",", custIds)).Returns(roleCustomers);
            _baseCCAgencyDataAccess.GetRescanCustomerClassificationAgency(string.Join(",", custIds)).Returns(custRescans);
            _dataAccess.PreNormalClassification(Arg.Any<string>(), paras.FunctionId, paras.RescanNormalType).Returns(1);
            _baseCCAgencyDataAccess.InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>())
                .Returns(new ResultInsertCustomerClassificationAgencyModel
                {
                    CustInfos = new List<CustInfoClassificationAgencyEntity>
                    {
                        new CustInfoClassificationAgencyEntity { CustID = 2 }
                    }
                });
            _baseCCAgencyDataAccess.UpdateBetLimitOfFlaggedCustomer(Arg.Any<string>()).Returns(new DBResult());
            _custClassToMainDBService.PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), paras.PushDataExternal).Returns(true);

            // Act
            var result = _service.RescanCustomerClassificationAgency(custIds, paras);

            // Assert
            result.Should().BeTrue();
            _dataAccess.Received(1).GetRoleCustomer(string.Join(",", custIds));
            _baseCCAgencyDataAccess.Received(1).GetRescanCustomerClassificationAgency(string.Join(",", custIds));
            _dataAccess.Received(1).PreNormalClassification(Arg.Any<string>(), paras.FunctionId, paras.RescanNormalType);
            _baseCCAgencyDataAccess.Received(1).InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>());
            _custClassToMainDBService.Received(2).PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>());
        }

        [Fact]
        public void RescanCustomerClassificationAgency_WhenNoCustRescans_ShouldReturnTrue()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3 };
            var paras = new RescanParametersModel
            {
                FunctionId = FunctionId.Default,
                IsToQueue = false,
                ServiceName = "TestService",
                PushDataExternal = new PushDataToExternalModel()
            };

            var custRescans = new GetRescanCustomerClassificationAgencyModel
            {
                CustInfos = Enumerable.Empty<RescanCustomerClassificationAgencyEntity>()
            };

            var roleCustomers = new List<RoleCustomerEntity>
            {
                new RoleCustomerEntity { CustID = 1, RoleID = (int)CustomerRole.Agent },
                new RoleCustomerEntity { CustID = 2, RoleID = (int)CustomerRole.Master }
            };

            _dataAccess.GetRoleCustomer(string.Join(",", custIds)).Returns(roleCustomers);
            _baseCCAgencyDataAccess.GetRescanCustomerClassificationAgency(string.Join(",", custIds)).Returns(custRescans);
            _custClassToMainDBService.PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), paras.PushDataExternal).Returns(true);

            // Act
            var result = _service.RescanCustomerClassificationAgency(custIds, paras);

            // Assert
            result.Should().BeTrue();
            _dataAccess.Received(1).GetRoleCustomer(string.Join(",", custIds));
            _baseCCAgencyDataAccess.Received(1).GetRescanCustomerClassificationAgency(string.Join(",", custIds));
            _custClassToMainDBService.Received(1).PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), paras.PushDataExternal);
        }

        [Fact]
        public void RescanCustomerClassificationAgency_WhenCustClassToMainDBServiceReturnsFalse_ShouldReturnFalse()
        {
            // Arrange
            var custIds = new List<long> { 1, 2, 3 };
            var paras = new RescanParametersModel
            {
                FunctionId = FunctionId.Default,
                IsToQueue = false,
                ServiceName = "TestService",
                PushDataExternal = new PushDataToExternalModel()
            };

            var custRescans = new GetRescanCustomerClassificationAgencyModel
            {
                CustInfos = Enumerable.Empty<RescanCustomerClassificationAgencyEntity>()
            };

            var roleCustomers = new List<RoleCustomerEntity>
            {
                new RoleCustomerEntity { CustID = 1, RoleID = (int)CustomerRole.Agent },
                new RoleCustomerEntity { CustID = 2, RoleID = (int)CustomerRole.Master }
            };

            _dataAccess.GetRoleCustomer(string.Join(",", custIds)).Returns(roleCustomers);
            _baseCCAgencyDataAccess.GetRescanCustomerClassificationAgency(string.Join(",", custIds)).Returns(custRescans);
            _custClassToMainDBService.PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), paras.PushDataExternal).Returns(false);

            // Act
            var result = _service.RescanCustomerClassificationAgency(custIds, paras);

            // Assert
            result.Should().BeFalse();
            _custClassToMainDBService.Received(1).PushExternalProblemAcountPackage(Arg.Any<IEnumerable<long>>(), paras.PushDataExternal);
        }
       
        #endregion       
    }
}
