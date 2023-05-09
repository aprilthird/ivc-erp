using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class ProjectFormulaActivity
    {
        public Guid Id { get; set; }
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormula ProjectFormula { get; set; }
        public string Description { get; set; }
        public Guid? MeasurementUnitId { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
    }
}
