using Fanex.NAP.SPU.CTS.AgencyCC.Features.AntiFraudCenter;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency;
using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgency.Entities;
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
using Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Extensions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using Xunit;
using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;
using static Fanex.NAP.SPU.CTS.AgencyCC.Tests.Infrastructure.Consts.CustomerClassificationAgency.AffectDownlineCustomerClassification.NewCustomerClassificationAgencyConsts;

namespace Fanex.NAP.SPU.CTS.AgencyCC.Tests.Features.CustomerClassificationAgency.AffectDownlineCustomerClassification
{
    public class NewCustomerClassificationAgencyServiceTest
    {
        private readonly IProblemAccountAgencyService _problemAccountAgencyService;
        private readonly ICustClassToMainDBService _custClassToMainDBService;
        private readonly IRescanCustClassificationAgencyService _rescanCustClassificationAgencyService;
        private readonly IAntiFraudCenterServices _antiFraudCenterServices;
        private readonly IBaseCustomerClassificationAgencyService _baseCustomerClassificationAgencyService;

        private readonly INewCustomerClassificationAgencyService _service;

        public NewCustomerClassificationAgencyServiceTest()
        {
            _problemAccountAgencyService = Substitute.For<IProblemAccountAgencyService>();
            _custClassToMainDBService = Substitute.For<ICustClassToMainDBService>();
            _rescanCustClassificationAgencyService = Substitute.For<IRescanCustClassificationAgencyService>();
            _antiFraudCenterServices = Substitute.For<IAntiFraudCenterServices>();
            _baseCustomerClassificationAgencyService = Substitute.For<IBaseCustomerClassificationAgencyService>();

            _service = new NewCustomerClassificationAgencyService(
                                _baseCustomerClassificationAgencyService,
                                _problemAccountAgencyService,
                                _custClassToMainDBService,
                                _rescanCustClassificationAgencyService,
                                _antiFraudCenterServices);
        }

        #region InsertVVIPForNewCustomerAgency
        [Fact]
        public void InsertVVIPForNewCustomerAgency_WhenSuccess()
        {
            // Arrange
            var newCustomerClassificationAgencyModel = new NewCustomerClassificationAgencyModel
            {
                VVIPClassification = new List<VvipCategoryAgencyEntity>
                {
                    new VvipCategoryAgencyEntity()
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
                VVIPClassification = new List<VvipCategoryAgencyEntity>
                {
                    new VvipCategoryAgencyEntity()
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
            var paClassification = new List<InsertProblemAccountEntity>
            {
                new InsertProblemAccountEntity(new RescanCustomerClassificationAgencyEntity())
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
                                                Arg.Any<List<InsertProblemAccountEntity>>(),
                                                Arg.Any<FunctionId>(),
                                                Arg.Any<AgencyCCInputFlow>(),
                                                Arg.Any<bool>(), Arg.Any<string>()).Returns(new List<long> { 1, 2, 3, 4 });

            _custClassToMainDBService.PushExternalProblemAcountPackage(Arg.Any<List<long>>(), Arg.Any<PushDataToExternalModel>()).Returns(true);

            _service.InsertPAForNewCustomerAgency(new RescanParametersModel());

            var result = isSuccssField.GetValue(_service);

            // Assert
            _problemAccountAgencyService.Received(1).InsertProblemAccountsAgencyPackage(
                                                Arg.Any<List<InsertProblemAccountEntity>>(),
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
            var paClassification = new List<InsertProblemAccountEntity>
            {
                new InsertProblemAccountEntity(new RescanCustomerClassificationAgencyEntity())
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
                                                Arg.Any<List<InsertProblemAccountEntity>>(),
                                                Arg.Any<FunctionId>(),
                                                Arg.Any<AgencyCCInputFlow>(),
                                                Arg.Any<bool>(), Arg.Any<string>()).Throws<Exception>();

            Action action = () => _service.InsertPAForNewCustomerAgency(new RescanParametersModel());

            // Assert
            _problemAccountAgencyService.Received(0).InsertProblemAccountsAgencyPackage(
                                                Arg.Any<List<InsertProblemAccountEntity>>(),
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
    }
}
