using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.SewerManifoldFor24ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.For24ExtrasViewModels
{
    public class For24SecondPartEquipmentViewModel
    {
        public Guid? Id { get; set; }
        public Guid SewerManifoldFor24SecondPartId { get; set; }
        public SewerManifoldFor24SecondPartViewModel SewerManifoldFor24SecondPart { get; set; }
        [Display(Name = "Nombre del Equipo", Prompt = "Nombre del Equipo")]
        public string EquipmentName { get; set; }
        [Display(Name ="Cantidad", Prompt = "Cantidad")]
        public int EquipmentQuantity { get; set; }
        [Display(Name = "Horas Máquina", Prompt = "Horas Máquina")]
        public string EquipmentHours { get; set; }
        [Display(Name ="Total Horas", Prompt = "Total Horas")]
        public string EquipmentTotalHours { get; set; }
    }
}
