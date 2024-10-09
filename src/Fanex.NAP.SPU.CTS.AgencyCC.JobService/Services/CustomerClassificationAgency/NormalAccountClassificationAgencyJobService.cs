namespace Fanex.NAP.SPU.CTS.AgencyCC.JobService.Services.CustomerClassificationAgency
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Entities;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountClassificationAgency.Model;
    using Fanex.NAP.SPU.CTS.AgencyCC.Shares;
    using Sentry;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface INormalAccountClassificationAgencyJobService
    {
        Task<bool> Execute(int batchInternalSize);
    }

    public class NormalAccountClassificationAgencyJobService : INormalAccountClassificationAgencyJobService
    {
        private readonly string _serviceName = "NormalAccountClassificationAgencyService";
        private readonly ISentryClient _sentryClient;
        private readonly INormalAccountClassificationAgencyService _service;

        public NormalAccountClassificationAgencyJobService(INormalAccountClassificationAgencyService service, ISentryClient sentryClient)
        {
            _service = service;
            _sentryClient = sentryClient;
        }

        public Task<bool> Execute(int batchInternalSize)
        {
            try
            {
                if (batchInternalSize <= 0)
                    batchInternalSize = 5000;

                JobRun(batchInternalSize);
            }
            catch (Exception ex)
            {
                Utilities.LogSentryError(_serviceName, ex.Message, string.Empty, ex, _sentryClient);

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        private bool JobRun(int batchInternalSize)
        {
            var normalPoolCustomerModel = _service.GetNormalPoolCustomers();

            var isExistCusts = normalPoolCustomerModel != null && normalPoolCustomerModel.NormalPoolCustomers.Any();

            if (isExistCusts)
            {
                ClassifyNormalAccountWithBatch(normalPoolCustomerModel, batchInternalSize);
            }

            return isExistCusts;
        }

        private void ClassifyNormalAccountWithBatch(GetNormalPoolCustomerAgencyModel normalPoolCustomer, int batchInternalSize)
        {
            var customerList = normalPoolCustomer.NormalPoolCustomers;

            if (customerList.IsNotNullOrEmpty())
            {
                var custChunks = customerList.Chunk(batchInternalSize).ToList();

                custChunks.ForEach(custBatch => ClassifyNormalAccount(custBatch, normalPoolCustomer.ScannedMaxId));
            }
        }

        private void ClassifyNormalAccount(IEnumerable<GetNormalPoolCustomerAgencyEntity> normalPoolCustomers, long scannedMaxID)
        {
            var custIDList = string.Join(",", normalPoolCustomers.Select(x => x.CustId).Distinct());

            var customerCategories = _service.GetCurrentCustomerCategory(custIDList);

            var customerInfos = normalPoolCustomers.Join(customerCategories, custMain => custMain.CustId,
                                                  custCTS => custCTS.CustID,
                                                  (custMain, custCTS) => new GetCurrentCustomerCategoryAgencyModel
                                                  {
                                                      CustId = custMain.CustId,
                                                      CategoryId = custCTS.CategoryID,
                                                      IsNewCreated = custCTS.IsNewCreated,
                                                      IsRealtimeOnly = custMain.IsRealtimeOnly,
                                                      RoleId = custCTS.RoleID
                                                  });

            _service.ClassifyNormalPoolCustomers(customerInfos.ToXmlString())
                                .ClearNormalPoolCustomers(custIDList, scannedMaxID);
        }
    }
}
