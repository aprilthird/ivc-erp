using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerPayrollViewModels
{
    public class WorkerPayrollDetailViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public Guid PayrollMovementHeaderId { get; set; }
        [Required]
        public Guid WorkerId { get; set; }
        [Required]
        public Guid? PayrollConceptId { get; set; }
        public PayrollConceptViewModel PayrollConcept { get; set; }

        public decimal Value { get; set; }
    }
}
