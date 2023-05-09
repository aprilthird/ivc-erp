using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementDetailViewModels
{
    public class PayrollMovementDetailListViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public Guid PayrollMovementHeaderId { get; set; }

        [Required]
        public Guid WorkerId { get; set; }

        public WorkerViewModel Worker { get; set; }

        public IEnumerable<PayrollMovementDetailViewModel> Details { get; set; }
    }
}
