using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachViewModels
{
    public class EquipmentMachSOATFoldingViewModel
    {
        public Guid? Id { get; set; }


        public Guid EquipmentMachId { get; set; }

        public EquipmentMachViewModel EquipmentMach { get; set; }

        [Display(Name = "Fecha de Inicio SOAT", Prompt = "Fecha de Inicio SOAT")]
        public string StartDateSoat { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        [Display(Name = "Fecha de Fin SOAT", Prompt = "Fecha de Fin SOAT")]
        public string EndDateSoat { get; set; } = DateTime.UtcNow.Date.ToShortDateString();

        public int Validity { get; set; }
        public Uri SoatFileUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile SoatFile { get; set; }

        [Display(Name = "Encargado(s)", Prompt = "Encargado(s)")]
        public IEnumerable<String> ResponsiblesSoat { get; set; }

        public int OrderSoat { get; set; }
    }
}
