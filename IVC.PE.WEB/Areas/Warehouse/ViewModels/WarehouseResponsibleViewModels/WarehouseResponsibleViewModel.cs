using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseResponsibleViewModels
{
    public class WarehouseResponsibleViewModel
    {
        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid? ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        [Display(Name = "Control Oficina Técnica", Prompt = "Control Oficina Técnica")]
        public IEnumerable<string> ThecnicalOfficeIds { get; set; }
        public string ThecnicalOfficeNames { get; set; }

        [Display(Name = "Solicitud de Pedidos", Prompt = "Solicitud de Pedidos")]
        public IEnumerable<string> OrderRequestIds { get; set; }
        public string OrderRequestNames { get; set; }

        [Display(Name = "Almaceneros", Prompt = "Almaceneros")]
        public IEnumerable<string> StoreKeeperIds { get; set; }
        public string StoreKeeperNames { get; set; }
    }
}
