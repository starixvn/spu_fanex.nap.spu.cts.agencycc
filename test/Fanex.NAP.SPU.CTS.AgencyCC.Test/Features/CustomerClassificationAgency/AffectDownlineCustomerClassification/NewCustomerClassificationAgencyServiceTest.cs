using Fanex.NAP.SPU.CTS.AgencyCC.Features.AntiFraudCenter;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Entitites;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.ProblemAccountAgencyInsert.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.RescanCustomerClassificationAgency.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.PushDataToExternal.CustClassificationToMainDB.Model;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter;
using Fanex.NAP.SPU.CTS.AgencyCC.Test.Infrastructure.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sentry;
using System;
using System.Collections.Generic;
using Xunit;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;
using static Fanex.NAP.SPU.CTS.AgencyCC.Test.Infrastructure.Consts.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgencyConsts;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Test.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    public class NewCustomerClassificationAgencyServiceTest
    {
        private readonly INewCustomerClassificationAgencyDataAccess _dataAccess;
        private readonly IProblemAccountAgencyService _problemAccountAgencyService;
        private readonly ICustClassToMainDBService _custClassToMainDBService;
        private readonly IRescanCustClassificationAgencyService _rescanCustClassificationAgencyService;
        private readonly ISystemParameterServices _systemParameterServices;
        private readonly IAntiFraudCenterServices _antiFraudCenterServices;
        private readonly IBaseCustomerClassificationAgencyService _baseCustomerClassificationAgencyService;

        private readonly INewCustomerClassificationAgencyService _service;

        public NewCustomerClassificationAgencyServiceTest()
        {
            _dataAccess = Substitute.For<INewCustomerClassificationAgencyDataAccess>();
            _problemAccountAgencyService = Substitute.For<IProblemAccountAgencyService>();
            _custClassToMainDBService = Substitute.For<ICustClassToMainDBService>();
            _rescanCustClassificationAgencyService = Substitute.For<IRescanCustClassificationAgencyService>();
            _systemParameterServices = Substitute.For<ISystemParameterServices>();
            _antiFraudCenterServices = Substitute.For<IAntiFraudCenterServices>();
            _baseCustomerClassificationAgencyService = Substitute.For<IBaseCustomerClassificationAgencyService>();

            _service = new NewCustomerClassificationAgencyService(
                                _baseCustomerClassificationAgencyService,
                                _dataAccess, _problemAccountAgencyService, _custClassToMainDBService,
                                _rescanCustClassificationAgencyService, _systemParameterServices,
                                _antiFraudCenterServices);
        }

        #region GetCategoryForNewCTCustomerAgency
        [Fact]
        public void GetCategoryForNewCTCustomerAgency_WhenSuccess()
        {
            // Arrange
            var newCustomerClassificationAgencyModel = new NewCustomerClassificationAgencyModel
            {
                VVIPClassification = new List<VVIPCategoryAgencyEntity>(),
                PAClassification = new List<InsertProblemAccountAgencyModel>(),
                NormalClassification = new List<NormalRescanAgencyEntity>(),
                LastCTSCustID = 999
            };

            var newCustomerClassificationAgencyModelField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.NewCustomerClassificationAgencyModel);

            // Act
            _dataAccess.GetNewCTSCustomerCategoryAgency().Returns(newCustomerClassificationAgencyModel);

            _service.GetCategoryForNewCTSCustomerAgency();

            var result = newCustomerClassificationAgencyModelField.GetValue(_service);

            // Assert
            _dataAccess.Received(1).GetNewCTSCustomerCategoryAgency();

            Assert.Equal(result, newCustomerClassificationAgencyModel);
        }

        [Fact]
        public void GetCategoryForNewCTCustomerAgency_ThrowException()
        {
            // Arrange

            // Act
            _dataAccess.GetNewCTSCustomerCategoryAgency().Throws<Exception>();

            Action action = () => _service.GetCategoryForNewCTSCustomerAgency();

            // Assert
            _dataAccess.Received(0).GetNewCTSCustomerCategoryAgency();

            Assert.Throws<Exception>(action);
        }
        #endregion GetCategoryForNewCTCustomerAgency

        #region InsertVVIPForNewCustomerAgency
        [Fact]
        public void InsertVVIPForNewCustomerAgency_WhenSuccess()
        {
            // Arrange
            var newCustomerClassificationAgencyModel = new NewCustomerClassificationAgencyModel
            {
                VVIPClassification = new List<VVIPCategoryAgencyEntity>()
                {
                    new VVIPCategoryAgencyEntity()
                }
            };

            var isSuccessField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.IsSuccess);

            var newCustomerClassificationAgencyModelField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.NewCustomerClassificationAgencyModel);

            // Act
            isSuccessField.SetValue(_service, true);

            newCustomerClassificationAgencyModelField.SetValue(_service, newCustomerClassificationAgencyModel);

            _baseCustomerClassificationAgencyService
                .InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>(), Arg.Any<string>())
                .Returns(new ResultInsertCustomerClassificationAgencyModel()
                {
                    CustInfos = new List<CustInfoClassificationAgencyEntity>
                    {
                        new CustInfoClassificationAgencyEntity()
                    }
                });

            _custClassToMainDBService.PushExternalPackageByCustId(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>()).Returns(true);

            _service.InsertVVIPForNewCustomerAgency(new RescanParametersModel()
            {
                PushDataExternal = new PushDataToExternalModel()
            });

            // Assert
            _baseCustomerClassificationAgencyService.Received(1).InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>(), Arg.Any<string>());

            _custClassToMainDBService.Received(1).PushExternalPackageByCustId(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>());

            _antiFraudCenterServices.Received(1).DeleteAFCAccounts(Arg.Any<IEnumerable<long>>(), Arg.Any<FromFlow>(), Arg.Any<bool>(), Arg.Any<string>());
        }

        [Fact]
        public void InsertVVIPForNewCustomerAgency_ThrowException()
        {
            // Arrange
            var newCustomerClassificationAgencyModel = new NewCustomerClassificationAgencyModel
            {
                VVIPClassification = new List<VVIPCategoryAgencyEntity>()
                {
                    new VVIPCategoryAgencyEntity()
                }
            };

            var isSuccessField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.IsSuccess);

            var newCustomerClassificationAgencyModelField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.NewCustomerClassificationAgencyModel);

            // Act
            isSuccessField.SetValue(_service, true);

            newCustomerClassificationAgencyModelField.SetValue(_service, newCustomerClassificationAgencyModel);

            _baseCustomerClassificationAgencyService
                .InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>(), Arg.Any<string>())
                .Throws<Exception>();

            Action action = () => _service.InsertVVIPForNewCustomerAgency(new RescanParametersModel()
                                    {
                                        PushDataExternal = new PushDataToExternalModel()
                                    });

            // Assert
            _baseCustomerClassificationAgencyService.Received(0).InsertCustomerClassification(Arg.Any<AgencyCCInputFlow>(), Arg.Any<InsertCustomerClassificationAgencyModel>(), Arg.Any<string>());

            _custClassToMainDBService.Received(0).PushExternalPackageByCustId(Arg.Any<IEnumerable<long>>(), Arg.Any<PushDataToExternalModel>());

            _antiFraudCenterServices.Received(0).DeleteAFCAccounts(Arg.Any<IEnumerable<long>>(), Arg.Any<FromFlow>(), Arg.Any<bool>(), Arg.Any<string>());

            Assert.Throws<Exception>(action);
        }
        #endregion

        #region InsertPAForNewCustomerAgency
        [Fact]
        public void InsertPAForNewCustomerAgency_WhenSuccess()
        {
            // Arrange
            var paClassification = new List<InsertProblemAccountAgencyModel>
            {
                new InsertProblemAccountAgencyModel()
            };

            var isSuccssField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.IsSuccess);
            var newCustomerClassificationField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.NewCustomerClassificationAgencyModel);

            // Act
            isSuccssField.SetValue(_service, true); ;
            newCustomerClassificationField.SetValue(_service, new NewCustomerClassificationAgencyModel
            {
                PAClassification = paClassification
            });

            _problemAccountAgencyService.InsertProblemAccountsAgencyPackage(
                                                Arg.Any<List<InsertProblemAccountAgencyModel>>(),
                                                Arg.Any<FunctionId>(),
                                                Arg.Any<AgencyCCInputFlow>(),
                                                Arg.Any<bool>(), Arg.Any<string>()).Returns(new List<long> { 1, 2, 3, 4 });

            _custClassToMainDBService.PushExternalProblemAcountPackage(Arg.Any<List<long>>(), Arg.Any<PushDataToExternalModel>()).Returns(true);

            _service.InsertPAForNewCustomerAgency(new RescanParametersModel());

            var result = isSuccssField.GetValue(_service);

            // Assert
            _problemAccountAgencyService.Received(1).InsertProblemAccountsAgencyPackage(
                                                Arg.Any<List<InsertProblemAccountAgencyModel>>(),
                                                Arg.Any<FunctionId>(),
                                                Arg.Any<AgencyCCInputFlow>(),
                                                Arg.Any<bool>(), Arg.Any<string>());

            _custClassToMainDBService.Received(1).PushExternalProblemAcountPackage(Arg.Any<List<long>>(), Arg.Any<PushDataToExternalModel>());

            Assert.True((bool)result);
        }

        [Fact]
        public void InsertPAForNewCustomerAgency_ThrowException()
        {
            // Arrange
            var paClassification = new List<InsertProblemAccountAgencyModel>
            {
                new InsertProblemAccountAgencyModel()
            };

            var isSuccssField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.IsSuccess);
            var newCustomerClassificationField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.NewCustomerClassificationAgencyModel);

            // Act
            isSuccssField.SetValue(_service, true); ;
            newCustomerClassificationField.SetValue(_service, new NewCustomerClassificationAgencyModel
            {
                PAClassification = paClassification
            });

            _problemAccountAgencyService.InsertProblemAccountsAgencyPackage(
                                                Arg.Any<List<InsertProblemAccountAgencyModel>>(),
                                                Arg.Any<FunctionId>(),
                                                Arg.Any<AgencyCCInputFlow>(),
                                                Arg.Any<bool>(), Arg.Any<string>()).Throws<Exception>();

            Action action = () => _service.InsertPAForNewCustomerAgency(new RescanParametersModel());

            // Assert
            _problemAccountAgencyService.Received(0).InsertProblemAccountsAgencyPackage(
                                                Arg.Any<List<InsertProblemAccountAgencyModel>>(),
                                                Arg.Any<FunctionId>(),
                                                Arg.Any<AgencyCCInputFlow>(),
                                                Arg.Any<bool>(), Arg.Any<string>());

            _custClassToMainDBService.Received(0).PushExternalProblemAcountPackage(Arg.Any<List<long>>(), Arg.Any<PushDataToExternalModel>());

            Assert.Throws<Exception>(action);
        }
        #endregion

        #region RescanNormalForNewCustomerAgency
        [Fact]
        public void RescanNormalForNewCustomerAgency_WhenSuccess()
        {
            // Arrange
            var normalClassification = new List<NormalRescanAgencyEntity>
            {
                new NormalRescanAgencyEntity()
            };

            var isSuccssField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.IsSuccess);
            var newCustomerClassificationField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.NewCustomerClassificationAgencyModel);

            // Act
            isSuccssField.SetValue(_service, true); ;
            newCustomerClassificationField.SetValue(_service, new NewCustomerClassificationAgencyModel
            {
                NormalClassification = normalClassification
            });

            _rescanCustClassificationAgencyService.RescanCustomerClassificationAgency(Arg.Any<IEnumerable<long>>(), Arg.Any<RescanParametersModel>()).Returns(true);

            _service.RescanNormalForNewCustomerAgency(new RescanParametersModel());

            var result = isSuccssField.GetValue(_service);

            // Assert
            _rescanCustClassificationAgencyService.Received(1).RescanCustomerClassificationAgency(Arg.Any<IEnumerable<long>>(), Arg.Any<RescanParametersModel>());

            Assert.True((bool)result);
        }

        [Fact]
        public void RescanNormalForNewCustomerAgency_ThrowException()
        {
            // Arrange
            var normalClassification = new List<NormalRescanAgencyEntity>
            {
                new NormalRescanAgencyEntity()
            };

            var isSuccssField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.IsSuccess);
            var newCustomerClassificationField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.NewCustomerClassificationAgencyModel);

            // Act
            isSuccssField.SetValue(_service, true); ;
            newCustomerClassificationField.SetValue(_service, new NewCustomerClassificationAgencyModel
            {
                NormalClassification = normalClassification
            });

            _rescanCustClassificationAgencyService.RescanCustomerClassificationAgency(Arg.Any<IEnumerable<long>>(), Arg.Any<RescanParametersModel>()).Throws<Exception>();

            Action action = () => _service.RescanNormalForNewCustomerAgency(new RescanParametersModel());

            // Assert
            _rescanCustClassificationAgencyService.Received(0).RescanCustomerClassificationAgency(Arg.Any<IEnumerable<long>>(), Arg.Any<RescanParametersModel>());

            Assert.Throws<Exception>(action);
        }
        #endregion

        #region UpdateComplete
        [Fact]
        public void UpdateComplete_WhenSuccess()
        {
            // Arrange
            var newCustomerClassificationModelField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.NewCustomerClassificationAgencyModel);
            var isSuccessField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.IsSuccess);

            // Act
            newCustomerClassificationModelField.SetValue(_service, new NewCustomerClassificationAgencyModel
            {
                LastCTSCustID = 999
            });
            isSuccessField.SetValue(_service, true);

            _systemParameterServices.UpdateTimingExecution(Arg.Any<string>(), Arg.Any<CTS_SystemParameters>()).Returns(true);

            var result = _service.UpdateComplete();

            // Assert
            _systemParameterServices.Received(1).UpdateTimingExecution(Arg.Any<string>(), Arg.Any<CTS_SystemParameters>());

            Assert.True(result);
        }

        [Fact]
        public void UpdateComplete_ThrowException()
        {
            // Arrange
            var newCustomerClassificationModelField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.NewCustomerClassificationAgencyModel);
            var isSuccessField = TestExtension.GetPrivateFieldInfo(typeof(NewCustomerClassificationAgencyService), PrivateFieldName.IsSuccess);

            // Act
            newCustomerClassificationModelField.SetValue(_service, new NewCustomerClassificationAgencyModel
            {
                LastCTSCustID = 999
            });
            isSuccessField.SetValue(_service, true);

            _systemParameterServices.UpdateTimingExecution(Arg.Any<string>(), Arg.Any<CTS_SystemParameters>()).Throws<Exception>();

            Action action = () => _service.UpdateComplete();

            // Assert
            _systemParameterServices.Received(0).UpdateTimingExecution(Arg.Any<string>(), Arg.Any<CTS_SystemParameters>());

            Assert.Throws<Exception>(action);
        }
        #endregion
    }
}
