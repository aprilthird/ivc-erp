using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerWeeklyTaskViewModels
{
    public class WeeklyFilesUploadViewModel
    {
        [Display(Name = "Borrar información previa", Prompt = "Borrar información previa")]
        public bool DeletePreviousInfo { get; set; }

        public Guid WeekId { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Adjuntar archivos", Prompt = "Adjuntar archivos")]
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
