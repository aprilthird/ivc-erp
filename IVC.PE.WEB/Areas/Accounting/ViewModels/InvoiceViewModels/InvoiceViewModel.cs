using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Accounting.ViewModels.InvoiceViewModels
{
    public class InvoiceViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "N° de Serie", Prompt = "N° de Serie")]
        public string Serie { get; set; }

        [Display(Name = "Fecha de Emisión", Prompt = "Fecha de Emisión")]
        public string IssueDate { get; set; }

        public Uri FileUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

    }
}
