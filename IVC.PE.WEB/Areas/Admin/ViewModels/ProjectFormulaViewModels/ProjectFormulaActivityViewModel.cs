using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels
{
    public class ProjectFormulaActivityViewModel
    {
        public Guid? Id { get; set; }
        public Guid? ProjectFormulaId { get; set; }
        public ProjectFormulaViewModel ProjectFormula { get; set; }
        public string Description { get; set; }
        public Guid? MeasurementUnitId { get; set; }
        public MeasurementUnitViewModel MeasurementUnit { get; set; }
    }
}
