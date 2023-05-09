using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachinerySoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachinerySoftPartViewModels
{
    public class EquipmentMachinerySoftPartViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Tipo de Equipo Menor", Prompt = "Tipo de Equipo Menor")]
        public Guid EquipmentMachineryTypeSoftId { get; set; }
        public EquipmentMachineryTypeSoftViewModel EquipmentMachineryTypeSoft { get; set; }
        [Display(Name = "Proveedor", Prompt = "Proveedor")]
        public Guid EquipmentProviderId { get; set; }
        public EquipmentProviderViewModel EquipmentProvider { get; set; }
        [Display(Name = "Equipo Menor", Prompt = "Equipo Menor")]
        public Guid EquipmentMachinerySoftId { get; set; }
        public EquipmentMachinerySoftViewModel EquipmentMachinerySoft { get; set; }
        [Display(Name = "Asignado/Encargado", Prompt = "Asignado/Encargado")]
        public string UserId { get; set; }
        public string UserName { get; set; }
        
    }
}
