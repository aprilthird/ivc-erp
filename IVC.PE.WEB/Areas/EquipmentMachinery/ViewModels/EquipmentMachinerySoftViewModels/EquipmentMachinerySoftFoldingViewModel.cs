using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachinerySoftViewModels
{
    public class EquipmentMachinerySoftFoldingViewModel
    {
        public Guid? Id { get; set; }


        public Guid EquipmentMachinerySoftId { get; set; }

        public EquipmentMachinerySoftViewModel EquipmentMachinerySoft { get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string FreeDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        [Display(Name = "Descripción", Prompt = "Descripción")]

        public string FreeText { get; set; }

    }
}
