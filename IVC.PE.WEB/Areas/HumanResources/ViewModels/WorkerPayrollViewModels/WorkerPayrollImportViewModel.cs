using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerPayrollViewModels
{
    public class WorkerPayrollImportViewModel
    {
        [Display(Name = "Borrar información previa", Prompt = "Borrar información previa")]
        public bool DeletePreviousInfo { get; set; }
        public Guid WeekId { get; set; }
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
    }
}
