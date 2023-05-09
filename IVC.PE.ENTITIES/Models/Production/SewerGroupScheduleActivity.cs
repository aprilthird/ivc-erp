namespace IVC.PE.ENTITIES.Models.Production
{
    using System;
    using IVC.PE.ENTITIES.Models.General;
    using IVC.PE.ENTITIES.Models.TechnicalOffice;

    public class SewerGroupScheduleActivity
    {
        public Guid Id { get; set; }

        public Guid SewerGroupDailyScheduleId { get; set; }
        public SewerGroupSchedule SewerGroupDailySchedule { get; set; }

        public Guid SewerManifoldId { get; set; }
        public SewerManifold SewerManifold { get; set; }

        public Guid ProjectFormulaActivityId { get; set; }
        public ProjectFormulaActivity ProjectFormulaActivity { get; set; }
    }
}
