using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerWeeklyTaskViewModels
{
    public class WorkerWeeklyTaskImportViewModel
    {
        public Guid WeekId { get; set; }
        public IFormFile File { get; set; }
    }
}
