using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachPartViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryFuelMachPartViewModels
{
    public class EquipmentMachineryFuelMachPartViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Proveedor de Equipo", Prompt = "Proveedor de Equipo")]

        public Guid EquipmentProviderId { get; set; }

        public EquipmentProviderViewModel EquipmentProvider { get; set; }
        [Display(Name = "Equipo Maquinaria", Prompt = "Equipo Maquinaria")]
        public Guid EquipmentMachPartId { get; set; }
        public EquipmentMachPartViewModel EquipmentMachPart { get; set; }
        [Display(Name = "Galón Acumulado", Prompt = "Galón Acumulado")]
        public double AcumulatedGallon { get; set; }
    }
}
