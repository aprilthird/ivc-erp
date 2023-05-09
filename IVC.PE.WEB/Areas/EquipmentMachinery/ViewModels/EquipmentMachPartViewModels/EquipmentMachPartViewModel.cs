using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachPartViewModels
{
    public class EquipmentMachPartViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Tipo de Maquinaria", Prompt = "Tipo de Maquinaria")]
        public Guid EquipmentMachineryTypeTypeId { get; set; }
        public EquipmentMachineryTypeTypeViewModel EquipmentMachineryTypeType { get; set; }
        [Display(Name = "Proveedor", Prompt = "Proveedor")]
        public Guid EquipmentProviderId { get; set; }
        public EquipmentProviderViewModel EquipmentProvider { get; set; }
        [Display(Name = "Maquinaria", Prompt = "Maquinaria")]
        public Guid EquipmentMachId { get; set; }
        public EquipmentMachViewModel EquipmentMach{ get; set; }

        [Display(Name = "Mes", Prompt = "Mes")]
        public int Month { get; set; }
        [Display(Name = "Año", Prompt = "Año")]
        public int Year { get; set; }
        //[Display(Name = "Asignado/Encargado", Prompt = "Asignado/Encargado")]
        //public string UserId { get; set; }
        //public string UserName { get; set; }
    }
}
