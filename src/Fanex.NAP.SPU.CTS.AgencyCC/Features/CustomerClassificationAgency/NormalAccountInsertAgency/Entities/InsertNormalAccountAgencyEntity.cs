namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.NormalAccountInsertAgency.Entities
{
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.CustomerClassificationAgency.BaseCustomerClassificationAgency.Entities;

    public class InsertNormalAccountAgencyEntity : CustomerClassificationAgencyEntity
    {
        public InsertNormalAccountAgencyEntity(GetNormalAccountAgencyEntity model)
        {
            LastXDaysBetCount = model.LastXDaysBetCount;
            LastXDaysTurnoverRM = model.LastXDaysTurnoverRM;
            LastXDaysWinlossRM = model.LastXDaysWinlossRM;
            LastYDaysBetCount = model.LastYDaysBetCount;
            LastYDaysTurnoverRM = model.LastYDaysTurnoverRM;
            LastYDaysWinlossRM = model.LastYDaysWinlossRM;
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
