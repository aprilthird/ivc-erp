using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportPartViewModels
{
    public class EquipmentMachineryTransportPartViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Tipo de Transporte", Prompt = "Tipo de Transporte")]
        public Guid EquipmentMachineryTypeTransportId { get; set; }
        public EquipmentMachineryTypeTransportViewModel EquipmentMachineryTypeTransport { get; set; }
        [Display(Name = "Proveedor", Prompt = "Proveedor")]
        public Guid EquipmentProviderId { get; set; }
        public EquipmentProviderViewModel EquipmentProvider { get; set; }
        [Display(Name = "Transporte", Prompt = "Transporte")]
        public Guid EquipmentMachineryTransportId { get; set; }
        public EquipmentMachineryTransportViewModel EquipmentMachineryTransport { get; set; }

        [Display(Name = "Mes", Prompt = "Mes")]
        public int Month { get; set; }
        [Display(Name = "Año", Prompt = "Año")]
        public int Year { get; set; }
        //[Display(Name = "Asignado/Encargado", Prompt = "Asignado/Encargado")]
        //public string UserId { get; set; }
        //public string UserName { get; set; }
    }
}
