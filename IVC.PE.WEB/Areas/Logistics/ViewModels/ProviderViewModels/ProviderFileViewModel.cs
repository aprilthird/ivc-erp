using IVC.PE.CORE.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels
{
    public class ProviderFileViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Tipo", Prompt = "Tipo")]
        public int Type { get; set; }

        [Display(Name = "Url", Prompt = "Url")]
        public Uri FileUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
    }
}
