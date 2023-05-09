using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.TransportPhaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportPartViewModels
{
    public class EquipmentMachineryTransportPartFoldingViewModel
    {
        public Guid? Id { get; set; }

        public int Order { get; set; }

        public Guid EquipmentMachineryTransportPartId { get; set; }

        public EquipmentMachineryTransportPartViewModel EquipmentMachineryTransportPart { get; set; }
        [Display(Name = "# de parte", Prompt = "# de parte")]
        public string PartNumber { get; set; }
        [Display(Name = "Día", Prompt = "Día")]
        public string PartDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Operador", Prompt = "Operador")]
        public Guid? EquipmentMachineryOperatorId { get; set; }
        public EquipmentMachineryOperatorViewModel EquipmentMachineryOperator { get; set; }
        [Display(Name = "Kilometraje Inicial", Prompt = "Kilometraje Inicial")]
        public double InitMileage { get; set; }
        [Display(Name = "Kilometraje Final", Prompt = "Kilometraje Final")]
        public double EndMileage { get; set; }
        [Display(Name = "Especifico", Prompt = "Especifico")]
        public string Specific { get; set; }

        [Display(Name = "Asignado/Encargado", Prompt = "Asignado/Encargado")]
        public string UserId { get; set; }
        public string UserName { get; set; }

        public int WorkArea { get; set; }
        public string Activities { get; set; }
        public Guid? EquipmentMachineryTypeTransportActivityId { get; set; }

        [Display(Name = "Fase", Prompt = "Fase")]
        public Guid? TransportPhaseId { get; set; }
        public TransportPhaseViewModel TransportPhase { get; set; }
        public EquipmentMachineryTypeTransportActivityViewModel EquipmentMachineryTypeTransportActivity { get; set; }
        [Display(Name = "Actividades", Prompt = "Actividades")]
        public IEnumerable<Guid> EquipmentMachineryTypeTransportActivities { get; set; }
    }
}
