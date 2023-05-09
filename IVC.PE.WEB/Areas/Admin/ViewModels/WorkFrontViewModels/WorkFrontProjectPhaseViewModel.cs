using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels
{
    public class WorkFrontProjectPhaseViewModel
    {
        public Guid? Id { get; set; }

        public Guid WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }

        public Guid ProjectPhaseId { get; set; }
        public ProjectPhaseViewModel ProjectPhase { get; set; }

        public Guid FormulaId { get; set; }
        public string FormulaCode { get; set; }
    }
}
