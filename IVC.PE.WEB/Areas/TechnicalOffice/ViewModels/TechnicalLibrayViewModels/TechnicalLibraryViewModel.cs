using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SpecialityViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalLibrayViewModels
{
    public class TechnicalLibraryViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Especialidad", Prompt = "Especialidad")]
        public Guid? SpecialityId { get; set; }

        public SpecialityViewModel Speciality { get; set; }
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Autor", Prompt = "Autor")]
        public string Author { get; set; }
        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string LibraryDateStr { get; set; } = DateTime.UtcNow.ToShortDateString();

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        public Uri FileUrl { get; set; }
    }
}
