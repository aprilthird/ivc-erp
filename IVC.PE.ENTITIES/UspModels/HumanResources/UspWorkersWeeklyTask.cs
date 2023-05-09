using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkersWeeklyTask
    {
        public Guid WorkerId { get; set; }
        public string WorkerDocument { get; set; }
        public string WorkerFullName { get; set; }
        public int WorkerCategory { get; set; }
        public string WorkerCategoryStr => ConstantHelpers.Worker.Category.SHORT_VALUES[WorkerCategory];
        public string WorkerPosition { get; set; }
        public int WorkerOrigin { get; set; }
        public string WorkerOriginStr => ConstantHelpers.Worker.Origin.SHORT_VALUES[WorkerOrigin];
        public int WorkerWorkgroup { get; set; }
        public string WorkerWorkgroupStr => ConstantHelpers.Worker.Workgroup.VALUES[WorkerWorkgroup];
        public DateTime WorkerEntryDate { get; set; }
        public bool WorkerIsNew { get; set; }
        public bool WorkerIsActive { get; set; }
        public DateTime? WorkerCeaseDate { get; set; }
        public Guid SewerGroupId { get; set; }
        public string SewerGroupCode { get; set; }
        public string ProjectCollaborator { get; set; }
        public string ProjectCollaboratorBusinessName { get; set; }
        public Guid? WorkFrontHeadId { get; set; }
        public string WorkFrontHead { get; set; }
        public string ForemanEmployee { get; set; }
        public string ForemanWorker { get; set; }
        public bool IsHome { get; set; }

        //Monday
        public string MondayHN { get; set; }
        public decimal? MondayH60 { get; set; }
        public decimal? MondayH100 { get; set; }
        public string MondayPhase { get; set; }

        //Tuesday
        public string TuesdayHN { get; set; }
        public decimal? TuesdayH60 { get; set; }
        public decimal? TuesdayH100 { get; set; }
        public string TuesdayPhase { get; set; }

        //Wednesday
        public string WednesdayHN { get; set; }
        public decimal? WednesdayH60 { get; set; }
        public decimal? WednesdayH100 { get; set; }
        public string WednesdayPhase { get; set; }

        //Thursday
        public string ThursdayHN { get; set; }
        public decimal? ThursdayH60 { get; set; }
        public decimal? ThursdayH100 { get; set; }
        public string ThursdayPhase { get; set; }

        //Friday
        public string FridayHN { get; set; }
        public decimal? FridayH60 { get; set; }
        public decimal? FridayH100 { get; set; }
        public string FridayPhase { get; set; }

        //Saturday
        public string SaturdayHN { get; set; }
        public decimal? SaturdayH60 { get; set; }
        public decimal? SaturdayH100 { get; set; }
        public string SaturdayPhase { get; set; }

        //Sunday
        public string SundayH100 { get; set; }
        public string SundayPhase { get; set; }

        //Totals
        public decimal HN { get; set; }
        public decimal HD { get; set; }
        public decimal MR { get; set; }
        public decimal PL { get; set; }
        public decimal H60 { get; set; }
        public decimal H100 { get; set; }
        public decimal HML { get; set; }
        public int LS { get; set; }
        public int ML { get; set; }
        public int NA { get; set; }
        public int UPL { get; set; }
    }
}
