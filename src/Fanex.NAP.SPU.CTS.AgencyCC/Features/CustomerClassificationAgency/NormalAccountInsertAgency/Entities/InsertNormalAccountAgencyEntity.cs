namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Entities
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;

    public class InsertNormalAccountAgencyEntity : CustomerClassificationAgencyEntity
    {
        public InsertNormalAccountAgencyEntity(GetNormalAccountAgencyEntity model)
        {
            ActiveDays = model.ActiveDays;
            BetCount = model.BetCount;
            CategoryGroupID = model.CategoryGroupId;
            CategoryID = model.CategoryId;
            CustID = model.CustId;
            PerformanceTime = model.PerformanceTime;
            TurnoverRM = model.TurnoverRM;
            WinlossRM = model.WinlossRM;
        }
    }
}
