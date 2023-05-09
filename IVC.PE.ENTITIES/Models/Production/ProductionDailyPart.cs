using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;

namespace IVC.PE.ENTITIES.Models.Production
{
    public class ProductionDailyPart
    {
        public Guid Id { get; set; }

        public Guid ProjectFormulaId { get; set; }
        public ProjectFormula ProjectFormula { get; set; }
        
        public DateTime ReportDate { get; set; }

        public Guid WorkFrontHeadId { get; set; }
        public WorkFrontHead WorkFrontHead { get; set; }

        public Guid? WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public double Filling { get; set; }
        public double TheoreticalLayer { get; set; }
        public double FillLength { get; set; }
        public double ExcavatedLength { get; set; }
        public double InstalledLength { get; set; }
        public double RefilledLength { get; set; }
        public double GranularBaseLength { get; set; }
        public double Excavation { get; set; }
        public double Installation { get; set; }
        public double Filled { get; set; }
        public string Status { get; set; }
    }
}
