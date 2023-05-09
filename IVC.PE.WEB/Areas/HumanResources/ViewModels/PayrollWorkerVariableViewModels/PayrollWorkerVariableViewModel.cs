using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementHeaderViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarMonthViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarWeekViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollWorkerVariableViewModels
{
    public class PayrollWorkerVariableViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public Guid WorkerId { get; set; }

        public WorkerViewModel Worker { get; set; }

        [Required]
        public Guid PayrollVariableId { get; set; }

        public PayrollVariableViewModel PayrollVariable { get; set; }

        public Guid? PayrollMovementHeaderId { get; set; }

        public PayrollMovementHeaderViewModel PayrollMovementHeader { get; set; }

        public string Value { get; set; }
    }
}
