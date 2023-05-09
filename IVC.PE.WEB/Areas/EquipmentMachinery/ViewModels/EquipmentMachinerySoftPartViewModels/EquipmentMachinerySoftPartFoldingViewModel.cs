using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryOperatorViewModels;
using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeSoftViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachinerySoftPartViewModels
{
    public class EquipmentMachinerySoftPartFoldingViewModel
    {
        public Guid? Id { get; set; }


        public Guid EquipmentMachinerySoftPartId { get; set; }

        public EquipmentMachinerySoftPartViewModel EquipmentMachinerySoftPart { get; set; }
        [Display(Name = "# de parte", Prompt = "# de parte")]
        public string PartNumber { get; set; }
        [Display(Name = "Día", Prompt = "Día")]
        public string PartDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Operador", Prompt = "Operador")]
        public Guid EquipmentMachineryOperatorId { get; set; }
        public EquipmentMachineryOperatorViewModel EquipmentMachineryOperator { get; set; }
        [Display(Name = "Kilometraje Inicial", Prompt = "Kilometraje Inicial")]
        public string InitMileage { get; set; }
        [Display(Name = "Kilometraje Final", Prompt = "Kilometraje Final")]
        public string EndMileage { get; set; }
        [Display(Name = "Especifico", Prompt = "Especifico")]
        public string Specific { get; set; }


        public string Activities { get; set; }
        public Guid? EquipmentMachineryTypeSoftActivityId { get; set; }
        public EquipmentMachineryTypeSoftActivityViewModel EquipmentMachineryTypeSoftActivity { get; set; }
        [Display(Name = "Actividades", Prompt = "Actividades")]
        public IEnumerable<Guid> EquipmentMachineryTypeSoftActivities { get; set; }

        
    }
}
