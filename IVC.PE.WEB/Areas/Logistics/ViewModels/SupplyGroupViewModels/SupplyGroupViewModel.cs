using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyGroupViewModels
{
    public class SupplyGroupViewModel
    {
        [Display(Name = "Id")]
        public Guid? Id { get; set; }

        [Display(Name = "Familia", Prompt = "Familia")]
        public Guid? SupplyFamilyId { get; set; }

        public SupplyFamilyViewModel SupplyFamily { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Categoría (Filtro según Familia)", Prompt = "Categoría")]
        public int? Category { get; set; }

        public string FullName => $"{Code} - {Name}";
    }
}
