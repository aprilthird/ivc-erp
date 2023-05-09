using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentRegistryViewModels
{
    public class EquipmentViewModel
    {
        public Guid? Id { get; set; }
        
        [Display(Name = "Tipo de Equipo", Prompt = "Tipo de Equipo")]
        public string EquipmentType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "N° de Serie", Prompt = "N° de Serie")]
        public string SerialNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Modelo", Prompt = "Modelo")]
        public string Model { get; set; }
        
        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        [Display(Name = "Propietario", Prompt = "Propietario")]
        public string Propietary { get; set; }

        [Display(Name = "Operador", Prompt = "Operador")]
        public string Operator { get; set; }
        
        [Display(Name = "Estado", Prompt = "Estado")]
        public int Status { get; set; }
    }
}
