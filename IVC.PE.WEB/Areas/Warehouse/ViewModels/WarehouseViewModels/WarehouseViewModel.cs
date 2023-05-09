using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseTypeViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels
{
    public class WarehouseViewModel
    {
        public Guid? Id { get; set; }

        public Guid? ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        [Display(Name = "Tipo de Almacén", Prompt = "Tipo de Almacén")]
        public Guid WarehouseTypeId { get; set; }
        public WarehouseTypeViewModel WarehouseType { get; set; }

        [Display(Name = "Frente de Trabajo", Prompt = "Frente de Trabajo")]
        public Guid WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }

        [Display(Name = "Dirección", Prompt = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Vínculo de Google Maps", Prompt = "Vínculo de Google Maps")]
        public Uri GoogleMapsUrl { get; set; }
    }
}
