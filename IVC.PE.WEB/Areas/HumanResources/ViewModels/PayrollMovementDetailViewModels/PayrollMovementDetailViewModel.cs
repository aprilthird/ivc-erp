using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementHeaderViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementDetailViewModels
{
    public class PayrollMovementDetailViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public Guid PayrollMovementHeaderId { get; set; }

        public PayrollMovementHeaderViewModel PayrollMovementHeader { get; set; }

        [Required]
        public Guid WorkerId { get; set; }

        public WorkerViewModel Worker { get; set; }

        public Guid? PayrollConceptId { get; set; }

        public PayrollConceptViewModel PayrollConcept { get; set; }

        public decimal Value { get; set; }
    }
}
