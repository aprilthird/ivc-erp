using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachViewModels
{
    public class EquipmentMachTechnicalFoldingViewModel
    {
        public Guid? Id { get; set; }


        public Guid EquipmentMachId { get; set; }

        public EquipmentMachViewModel EquipmentMach { get; set; }

        [Display(Name = "Fecha de Inicio Seguro", Prompt = "Fecha de Inicio Seguro")]
        public string StartDateTechnical { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        [Display(Name = "Fecha de Fin Seguro", Prompt = "Fecha de Fin Seguro")]
        public string EndDateTechnical { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        public int Validity { get; set; }
        public Uri TechnicalFileUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile TechnicalFile { get; set; }

        [Display(Name = "Encargado(s)", Prompt = "Encargado(s)")]
        public IEnumerable<String> ResponsiblesTec { get; set; }

        public int OrderTechnical { get; set; }
    }
}
