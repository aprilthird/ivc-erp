namespace IVC.PE.ENTITIES.Models.Security
{
    using System;
    using IVC.PE.ENTITIES.Models.General;
    using IVC.PE.ENTITIES.Models.TechnicalOffice;
    
    public class BehaviourReport
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public bool ObsLessThan3Months { get; set; }
        public bool DayTurn { get; set; }
        public DateTime ObsDate { get; set; }
        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }
    }
}
