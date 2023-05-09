using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.PreRequestViewModels
{
    public class PreRequestFileViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Url", Prompt = "Url")]
        public Uri FileUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Adjuntar archivos", Prompt = "Adjuntar archivos")]
        public IEnumerable<IFormFile> PreRequestFiles { get; set; }
    }
}

