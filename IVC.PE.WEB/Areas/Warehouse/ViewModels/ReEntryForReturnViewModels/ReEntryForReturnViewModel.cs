using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.ReEntryForReturnViewModels
{
    public class ReEntryForReturnViewModel
    {
        public Guid? Id { get; set; }

        public string DocumentNumber { get; set; }

        [Display(Name = "Fórmula", Prompt = "Fórmula")]
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormulaViewModel ProjectFormula { get; set; }

        [Display(Name = "Frente", Prompt = "Frente")]
        public Guid WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }

        [Display(Name = "Cuadrilla", Prompt = "Cuadrilla")]
        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        //public Guid WarehouseId { get; set; }
        //public Warehouse Warehouse { get; set; }

        [Display(Name = "Familia", Prompt = "Familia")]
        public Guid SupplyFamilyId { get; set; }
        public SupplyFamilyViewModel SupplyFamily { get; set; }

        [Display(Name = "Fecha de Devolución", Prompt = "Fecha de Devolución")]
        public string ReturnDate { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public Uri FileUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        public int Status { get; set; }
    }
}
