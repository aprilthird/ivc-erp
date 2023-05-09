using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Aggregation.ViewModels.QuarryViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DrainageNetworkSummaryViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.CompactionDensityCertificateViewModels
{
    public class CompactionDensityCertificateViewModel
    {
        public Guid? Id { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "N° de Registro", Prompt = "N° de Registro")]
        public string SerialNumber { get; set; }

        [Display(Name = "Fecha de Termino de Relleno", Prompt = "Fecha de Termino de Relleno")]
        public string ExecutionDate { get; set; }

        [Display(Name = "Tramo", Prompt = "Tramo")]
        public Guid SewerLineId { get; set; }

        public SewerLineViewModel SewerLine { get; set; }
        
        [Display(Name = "Clase de Material", Prompt = "Clase de Material")]
        public int MaterialType { get; set; }

        [Display(Name = "Cantera", Prompt = "Cantera")]
        public Guid QuarryId { get; set; }

        public QuarryViewModel Quarry { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        public Uri FileUrl { get; set; }

        public IEnumerable<CompactionDensityCertificateDetailViewModel> Details { get; set; }
    }
}
