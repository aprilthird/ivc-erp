using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SpecialityViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalSpecViewModels
{
    public class TechnicalSpecViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Familia", Prompt = "Familia")]
        public Guid SupplyFamilyId { get; set; }
        public SupplyFamilyViewModel SupplyFamily { get; set; }
        [Display(Name = "Grupo", Prompt = "Grupo")]
        public Guid SupplyGroupId { get; set; }
        public SupplyGroupViewModel SupplyGroup { get; set; }
        //[Display(Name = "Especialidad", Prompt = "Especialidad")]
        //public Guid? SpecialityId { get; set; }
        //public SpecialityViewModel Speciality { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
        public Uri FileUrl { get; set; }

        [Display(Name = "Especialidades", Prompt = "Especialidades")]
        public IEnumerable<Guid> Specialities { get; set; }
    }
}
