using IVC.PE.CORE.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.WorkbookViewModels
{
    public class WorkbookViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Número", Prompt = "Número")]
        public int Number { get; set; }


        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }


        [Display(Name = "Rango  (Asiento 1 - Asiento 1000)", Prompt = "Rango  (Asiento 1 - Asiento 1000)")]
        public string Range { get; set; }


        [Display(Name = "Período  (dia/mes/año - dia/mes/año)", Prompt = "Período  (dia/mes/año - dia/mes/año)")]
        public string Term { get; set; }


        [Display(Name = "Archivo PDF", Prompt = "Archivo PDF")]
        public IFormFile File { get; set; }

        public Uri FileUrl { get; set; }
        
    }
}
