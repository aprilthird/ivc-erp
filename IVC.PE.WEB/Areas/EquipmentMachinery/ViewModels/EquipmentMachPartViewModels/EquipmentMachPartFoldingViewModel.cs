using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.MachineryPhaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachPartViewModels
{
    public class EquipmentMachPartFoldingViewModel
    {
        public Guid? Id { get; set; }

        public int Order { get; set; }

        public Guid EquipmentMachPartId { get; set; }

        public EquipmentMachPartViewModel EquipmentMachPart { get; set; }
        [Display(Name = "# de parte", Prompt = "# de parte")]
        public string PartNumber { get; set; }
        [Display(Name = "Día", Prompt = "Día")]
        public string PartDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Operador", Prompt = "Operador")]
        public Guid EquipmentMachineryOperatorId { get; set; }
        public EquipmentMachineryOperatorViewModel EquipmentMachineryOperator { get; set; }
        [Display(Name = "Horometro Inicial", Prompt = "Horometro Inicial")]
        public double InitHorometer { get; set; }
        [Display(Name = "Horometro Final", Prompt = "Horometro Final")]
        public double EndHorometer { get; set; }

        public double Dif { get; set; }
        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Specific { get; set; }

        [Display(Name = "Asignado/Encargado", Prompt = "Asignado/Encargado")]
        public string UserId { get; set; }
        public string UserName { get; set; }

        public int? WorkArea { get; set; }
        public string Activities { get; set; }

        [Display(Name = "Actividad", Prompt = "Actividad")]

        public Guid EquipmentMachineryTypeTypeActivityId { get; set; }
        public EquipmentMachineryTypeTypeActivityViewModel EquipmentMachineryTypeTypeActivity { get; set;}

        [Display(Name = "Cuadrilla", Prompt = "Cuadrilla")]
        public Guid SewerGroupId { get; set; }

        public SewerGroupViewModel SewerGroup { get; set; }
        [Display(Name = "Fase", Prompt = "Fase")]
        public Guid? MachineryPhaseId { get; set; }
        public MachineryPhaseViewModel MachineryPhase { get; set; }

        public string Sewers { get; set; }

        public string Phases { get; set; }

        public Guid? ProjectCalendarWeekId { get; set; }

    }
}
