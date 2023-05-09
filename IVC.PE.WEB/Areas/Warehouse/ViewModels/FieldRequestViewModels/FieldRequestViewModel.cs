using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.SupplyFamilyViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.WarehouseViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.FieldRequestViewModels
{
    public class FieldRequestViewModel
    {
        public Guid? Id { get; set; }

        public string DocumentNumber { get; set; }

        [Display(Name = "Presupuesto", Prompt = "Presupuesto")]
        public Guid BudgetTitleId { get; set; }
        public BudgetTitleViewModel BudgetTitle { get; set; }
        /*
        [Display(Name = "Fórmula", Prompt = "Fórmula")]
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormulaViewModel ProjectFormula { get; set; }
        */
        [Display(Name = "Frente de Trabajo", Prompt = "Frente de Trabajo")]
        public Guid WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }

        [Display(Name = "Cuadrilla", Prompt = "Cuadrilla")]
        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }
        /*
        [Display(Name = "Almacén", Prompt = "Almacén")]
        public Guid WarehouseId { get; set; }
        public WarehouseViewModel Warehouse { get; set; }
        */
        //[Display(Name = "Familia", Prompt = "Familia")]
        //public Guid SupplyFamilyId { get; set; }
        //public SupplyFamilyViewModel SupplyFamily { get; set; }

        [Display(Name = "Fecha de Entrega", Prompt = "Fecha de Entrega")]
        public string DeliveryDate { get; set; }

        public int Status { get; set; }

        [Display(Name = "Observaciones", Prompt = "Observaciones")]
        public string Observation { get; set; }

        public string Groups { get; set; }

        public string IssuedUserId { get; set; }

        public IEnumerable<string> Items { get; set; }

        [Display(Name = "Órden de Trabajo", Prompt = "Descripción libre")]
        public string WorkOrder { get; set; }

        [Display(Name = "Fórmulas", Prompt = "Fórmulas")]
        public List<Guid> ProjectFormulaIds { get; set; }

        public string Formulas { get; set; }

        public string WorkFrontStr { get; set; }

        public string SewerGroupStr { get; set; }

        public Uri FileUrl { get; set; }

        [Display(Name = "Guía Escaneada", Prompt = "Guía Escaneada")]
        public IFormFile File { get; set; }

        public string UserName { get; set; }
    }
}
