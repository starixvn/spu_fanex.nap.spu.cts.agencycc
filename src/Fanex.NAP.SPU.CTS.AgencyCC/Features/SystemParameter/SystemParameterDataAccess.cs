namespace Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter.Criteria;
    using Fanex.NAP.SPU.CTS.AgencyCC.Features.SystemParameter.Entities;
    using static Fanex.NAP.SPU.CTS.AgencyCC.Shares.Constants;

    public interface ISystemParameterDataAccess
    {
        SystemParameterEntity GetPreviousTimingExecution(CTS_SystemParameters paramID);

        void UpdateTimingExecution(string paraValue, CTS_SystemParameters paramId);

        SystemParameterEntity GetPreviousTimingExecution(Main_SystemParameters paramID);

        void UpdateTimingExecution(string paraValue, Main_SystemParameters paramId);
    }

    public class SystemParameterDataAccess : ISystemParameterDataAccess
    {
        private readonly IDynamicRepository repository;

        public SystemParameterDataAccess()
        {
            repository = new DynamicRepository();
        }

        public SystemParameterEntity GetPreviousTimingExecution(CTS_SystemParameters paramID)
        {
            var criteria = new PreviousTimingExecutionGetDCCriteria
            {
                ParamID = (int)paramID
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.QueryEntity<SystemParameterEntity>(criteria);
            }
        }

        public void UpdateTimingExecution(string paraValue, CTS_SystemParameters paramId)
        {
            var criteria = new PreviousTimingExecutionUpdateDCCriteria
            {
                ParamId = (int)paramId,
                ParamValue = paraValue
            };

            repository.Execute(criteria);
        }

        public SystemParameterEntity GetPreviousTimingExecution(Main_SystemParameters paramID)
        {
            var criteria = new PreviousTimingExecutionGetMainCriteria
            {
                ParamID = (int)paramID
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.QueryEntity<SystemParameterEntity>(criteria);
            }
        }

        public void UpdateTimingExecution(string paraValue, Main_SystemParameters paramId)
        {
            var criteria = new PreviousTimingExecutionUpdateMainCriteria
            {
                ParamId = (int)paramId,
                ParamValue = paraValue
            };

            repository.Execute(criteria);
        }
    }
}
