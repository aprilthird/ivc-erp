using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentProviderViewModels
{
    public class EquipmentProviderFoldingViewModel
    {
        public Guid? Id { get; set; }

        public Guid EquipmentProviderId { get; set; }
        public EquipmentProviderViewModel EquipmentProvider { get; set; }

        [Display(Name = "Clase de Equipo", Prompt = "Clase de Equipo")]
        public Guid EquipmentMachineryTypeId { get; set; }
        public EquipmentMachineryTypeViewModel EquipmentMachineryType { get; set; }
        [Display(Name = "Tipo de Equipo Menor", Prompt = "Tipo de Equipo Menor")]
        public Guid? EquipmentMachineryTypeSoftId { get; set; }

        public EquipmentMachineryTypeSoftViewModel EquipmentMachineryTypeSoft { get; set; }
        [Display(Name = "Tipo de Maquinaria", Prompt = "Tipo de Maquinaria")]
        public Guid? EquipmentMachineryTypeTypeId { get; set; }
        public EquipmentMachineryTypeTypeViewModel EquipmentMachineryTypeType { get; set; }

        [Display(Name = "Tipo de Transporte", Prompt = "Tipo de Transporte")]
        public Guid? EquipmentMachineryTypeTransportId { get; set; }
        public EquipmentMachineryTypeTransportViewModel EquipmentMachineryTypeTransport { get; set; }

    }
}
