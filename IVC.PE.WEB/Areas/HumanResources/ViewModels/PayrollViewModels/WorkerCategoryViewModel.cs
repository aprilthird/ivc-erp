using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels
{
    public class WorkerCategoryViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public int WorkerCategoryId { get; set; }

        public string WorkerCategoryName => ConstantHelpers.Worker.Category.VALUES[WorkerCategoryId];

        public decimal DayWage { get; set; }

        public decimal BUCRate { get; set; }
    }
}
