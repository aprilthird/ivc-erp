namespace IVC.PE.WEB.Areas.Quality.ViewModels.SewerManifoldFor29ViewModels
{
    using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    public class SewerManifoldFor29ViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Tramos Ejecución", Prompt = "Tramos Ejecución")]
        public Guid SewerManifoldId { get; set; }
        public SewerManifoldViewModel SewerManifold { get; set; }
        public string For01ProtocolNumber { get; set; }
        [Display(Name = "Fecha de Asfaltado", Prompt = "Fecha de Asfaltado")]
        public string AsphaltDate { get; set; }
        [Display(Name = "Tipo de Asfalto")]
        public int AsphaltType { get; set; }
        [Display(Name = "Espesor", Prompt = "Espesor")]
        public int Thickness { get; set; }
        [Display(Name = "Área Asfaltada", Prompt = "Área Asfaltada")]
        public string AsphaltArea { get; set; }
        [Display(Name = "Área a Valorizar", Prompt = "Área a Valorizar")]
        public string AreaToValue { get; set; }
        public Uri FileUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
        public string Pavement2InReview { get; set; }
        public string Pavement3InReview { get; set; }
        public string Pavement3InMixedReview { get; set; }
    }
}
