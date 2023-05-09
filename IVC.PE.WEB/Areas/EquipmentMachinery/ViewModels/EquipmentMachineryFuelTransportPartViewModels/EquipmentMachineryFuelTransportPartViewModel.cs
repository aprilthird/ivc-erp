using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportPartViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryFuelTransportPartViewModels
{
    public class EquipmentMachineryFuelTransportPartViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Proveedor de Equipo", Prompt = "Proveedor de Equipo")]

        public Guid EquipmentProviderId { get; set; }

        public EquipmentProviderViewModel EquipmentProvider { get; set; }
        [Display(Name = "Equipo Transporte", Prompt = "Equipo Transporte")]
        public Guid EquipmentMachineryTransportPartId { get; set; }
        public EquipmentMachineryTransportPartViewModel EquipmentMachineryTransportPart { get; set; }
        [Display(Name = "Galón Acumulado", Prompt = "Galón Acumulado")]
        public double AcumulatedGallon { get; set; }


    }
}
