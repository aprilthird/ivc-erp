using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryCalendarWeekViewModels
{
    public class EquipmentMachineryCalendarWeekViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción")]
        public int WeekNumber { get; set; }

        public string Description { get; set; }
        [Display(Name = "Del", Prompt = "Del")]
        public string WeekStart { get; set; }
        [Display(Name = "Al", Prompt = "Al")]
        public string WeekEnd { get; set; }
        [Display(Name = "Año", Prompt = "Año")]
        public int Year { get; set; }


    }
}
