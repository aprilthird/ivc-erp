namespace IVC.PE.WEB.Areas.Quality.ViewModels.SewerManifoldFor05ViewModels
{

    using IVC.PE.WEB.Areas.Quality.ViewModels.DischargeManifoldViewModels;
    using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class SewerManifoldFor05ViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Tramos Ejecución", Prompt = "Tramos Ejecución")]
        public Guid SewerManifoldId { get; set; }
        public SewerManifoldViewModel SewerManifold { get; set; }
        [Display(Name = "Número Certificado", Prompt = "Número Certificado")]
        public string CertificateNumber { get; set; }
        [Display(Name = "Número Capa", Prompt = "Número Capa")]
        public string LayerNumber { get; set; }
        [Display(Name = "Fecha Ensayo", Prompt = "Fecha Ensayo")]
        public string TestDate { get; set; }
        [Display(Name = "Estado", Prompt = "Estado")]
        public string Status { get; set; }
        [Display(Name = "Fecha Envío", Prompt = "Fecha Envío")]
        public string ShippingDate { get; set; }
        public double Filling { get; set; }
        public int TheoreticalLayer { get; set; }
        public int LayersNumber { get; set; }
        public Uri FileUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
    }
}