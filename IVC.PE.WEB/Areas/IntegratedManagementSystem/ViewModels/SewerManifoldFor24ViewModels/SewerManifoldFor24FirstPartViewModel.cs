using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.For24ExtrasViewModels;
using IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.NewSIGProcessViewModels;
using IVC.PE.WEB.Areas.LegalTechnicalLibrary.ViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.ProviderViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.IntegratedManagementSystem.ViewModels.SewerManifoldFor24ViewModels
{
    public class SewerManifoldFor24FirstPartViewModel
    {
        public Guid? Id { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }
        [Display(Name = "PROCESO OBSERVADO", Prompt = "PROCESO OBSERVADO")]
        public Guid NewSIGProcessId { get; set; }
        public NewSIGProcessViewModel NewSIGProcess { get; set; }
        [Display(Name = "REPORTADO", Prompt = "REPORTADO")]
        public string ReportUserId { get; set; }
        public string ReportUserName { get; set; }
        [Display(Name = "FECHA", Prompt = "FECHA")]
        public string Date { get; set; }
        //------------------------Origen Del P/S NC:
        [Display(Name = "Tipo de Origen", Prompt = "Tipo de Origen")]
        public int OriginType { get; set; }
        [Display(Name = "Origen de la NC", Prompt = "Origen de la NC")]
        public int NCOrigin { get; set; }
        [Display(Name = "Lista de Cuadrillas", Prompt = "Lista de Cuadrillas")]
        public Guid? SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }
        [Display(Name = "Lista de Proveedores", Prompt = "Lista de Proveedores")]
        public Guid? ProviderId { get; set; }
        public ProviderViewModel Provider { get; set; }
        [Display(Name = "Cliente", Prompt = "Cliente")]
        public string Client { get; set; }
        //------------------------Descripción del Hallazgo:
        [Display(Prompt = "Describa el hallazgo")]
        public string Description { get; set; }
        //------------------------Si Aplica llene los siguientes datos
        [Display(Name = "Nombre del Producto", Prompt = "Nombre del Producto")]
        public string ProductName { get; set; }
        [Display(Name = "Cantidad", Prompt = "Cantidad")]
        public string Quantity { get; set; }
        [Display(Name = "Marca/Proveedor", Prompt = "Marca/Proveedor")]
        public string BrandProvider { get; set; }
        [Display(Name = "Codigo/Referencia", Prompt = "Codigo/Referencia")]
        public string CodeReference { get; set; }
        [Display(Name = "Fecha de Vencimiento", Prompt = "Fecha de Vencimiento")]
        public string ExpirationDate { get; set; }
        //------------------------Responsable en levantar
        [Display(Name = "RESPONSABLE", Prompt = "RESPONSABLE")]
        public string ResponsableUserId { get; set; }
        public string ResponsableUserName { get; set; }
        //------------------------Regristro fotográfico
        public Uri FileUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
        public Uri VideoUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Video", Prompt = "Video")]
        public IFormFile Video { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Fotos", Prompt = "Fotos")]
        [JsonIgnore]
        public IFormFileCollection GalleryFiles { get; set; }
        public List<For24FirstPartGalleryViewModel> Gallery { get; set; }
    }
}
