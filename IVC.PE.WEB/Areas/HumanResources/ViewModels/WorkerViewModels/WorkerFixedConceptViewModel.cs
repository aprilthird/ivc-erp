using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels
{
    public class WorkerFixedConceptViewModel
    {
        public Guid? Id { get; set; }
        public Guid WorkerId { get; set; }
        public WorkerViewModel Worker { get; set; }
        [Display(Name = "Concepto", Prompt = "Concepto")]
        public Guid PayrollConceptId { get; set; }
        public PayrollConceptViewModel PayrollConcept { get; set; }
        [Display(Name = "Monto Fijo", Prompt = "Monto Fijo")]
        public string FixedValue { get; set; }
    }
}
