namespace IVC.PE.WEB.Areas.Quality.ViewModels.SewerManifoldFor47ViewModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using IVC.PE.CORE.Helpers;
    using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
    using Microsoft.AspNetCore.Http;

    public class SewerManifoldFor47ViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name ="Tramos Ejecución", Prompt = "Tramos Ejecución")]
        public Guid SewerManifoldId { get; set; }
        public SewerManifoldViewModel SewerManifold { get; set; }

        [Display(Name = "Normal", Prompt = "Normal")]
        public string LengthOfDiggingN { get; set; }
        [Display(Name = "Semirrocoso", Prompt = "Semirrocoso")]
        public string LengthOfDiggingSR { get; set; }
        [Display(Name = "Rocoso", Prompt = "Rocoso")]
        public string LengthOfDiggingR { get; set; }

        [Display(Name = "Nº Cuaderno de Trabajo", Prompt = "Nº Cuaderno de Trabajo")]
        public string WorkBookNumber { get; set; }
        [Display(Name = "Nº de Asiento", Prompt = "Nº de Asiento")]
        public string WorkBookSeat { get; set; }
        [Display(Name = "Fecha de Registro", Prompt = "Fecha de Registro")]
        public string WorkBookRegistryDate { get; set; }
        public Uri FileUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        public string For01ProtocolNumber { get; set; }
        public int BZiRealTerrainType { get; set; }
        public int BZjRealTerrainType { get; set; }
    }
}
