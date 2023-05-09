using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.AggregateTypeViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.CementTypeViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ConcreteUseViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DesignTypeViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalVersionViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.MixDesignViewModels
{
    public class MixDesignViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Tipo de Diseño", Prompt = "Tipo de Diseño")]
        public Guid DesignTypeId { get; set; }
        public DesignTypeViewModel DesignType { get; set; }

        [Display(Name = "Tipo de Cemento", Prompt = "Tipo de Cemento")]
        public Guid CementTypeId { get; set; }
        public CementTypeViewModel CementType { get; set; }
        [Display(Name = "Tipo de Agregado", Prompt = "Tipo de Agregado")]
        public Guid AggregateTypeId { get; set; }
        public AggregateTypeViewModel AggregateType { get; set; }
        [Display(Name = "Uso de Concreto", Prompt = "Uso de Concreto")]
        public Guid ConcreteUseId { get; set; }
        public ConcreteUseViewModel ConcreteUse { get; set; }
        [Display(Name = "¿Aditivo?", Prompt = "¿Aditivo?")]
        public bool Additive { get; set; }
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }
        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string DesignDateStr { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Versión", Prompt = "Versión")]
        public Guid TechnicalVersionId { get; set; }

        public TechnicalVersionViewModel TechnicalVersion { get; set; }
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public Uri FileUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
    }
}
