using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Production
{
    [NotMapped]
    public class UspProductionDailyPart
    {
        public Guid Id { get; set; }
        public string ProjectFormula { get; set; }
        public DateTime ReportDate { get; set; }
        public string ReportDateStr => ReportDate.ToDateString();
        public Guid WorkFrontHeadId { get; set; }
        public string WorkFrontHead { get; set; }
        public Guid? WorkFrontId { get; set; }
        public string WorkFront { get; set; }
        public Guid SewerGroupId { get; set; }
        public string SewerGroup { get; set; }
        public string SewerManifold { get; set; }
    }
}
