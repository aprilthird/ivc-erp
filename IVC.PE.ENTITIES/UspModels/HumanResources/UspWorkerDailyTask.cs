using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkerDailyTask
    {
        public Guid Id { get; set; }
        public string WorkerDocument { get; set; }
        public int WorkerCategory { get; set; }
        public string WorkerCategoryStr => ConstantHelpers.Worker.Category.VALUES[WorkerCategory];
        public string WorkerFullName { get; set; }
        public string ProjectPhaseFullDescription { get; set; }
        public string SewerGroupCode { get; set; }
        public string HoursNormal { get; set; }
        public decimal Hours60Percent { get; set; }
        public decimal Hours100Percent { get; set; }
    }
}
