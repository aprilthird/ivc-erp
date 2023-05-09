using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.MeteredsRestatedByPartidaViewModels
{
    public class MeteredsRestatedByPartidaViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Jefes de Frente")]
        public Guid WorkFrontHeadId { get; set; }
        public WorkFrontHeadViewModel WorkFrontHead { get; set; }

        [Display(Name = "Títulos de Presupuestos")]
        public Guid BudgetTitleId { get; set; }
        public BudgetTitleViewModel BudgetTitle { get; set; }

        [Display(Name = "Frentes")]
        public Guid WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }

        [Display(Name = "Cuadrillas")]
        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        public string ItemNumber { get; set; }

        public string Description { get; set; }

        public string Unit { get; set; }

        public string Metered { get; set; }

        public string UnitPrice { get; set; }

        public string Parcial { get; set; }

        public string AccumulatedMetered { get; set; }

        public string AccumulatedAmount { get; set; }

        public string F5_C01Metered { get; set; }

        public string F5_C01Amount { get; set; }

        public string F5_C01AMetered { get; set; }

        public string F5_C01AAmount { get; set; }

        public string F5_C02Metered { get; set; }

        public string F5_C02Amount { get; set; }

        public string F5_C02AMetered { get; set; }

        public string F5_C02AAmount { get; set; }

        public string F5_C03Metered { get; set; }

        public string F5_C03Amount { get; set; }

        public string F5_C03AMetered { get; set; }

        public string F5_C03AAmount { get; set; }

        public string F5_C04Metered { get; set; }

        public string F5_C04Amount { get; set; }

        public string F5_C04AMetered { get; set; }

        public string F5_C04AAmount { get; set; }

        public string F5_C05Metered { get; set; }

        public string F5_C05Amount { get; set; }

        public string F5_C05AMetered { get; set; }

        public string F5_C05AAmount { get; set; }

        public string F5_C06Metered { get; set; }

        public string F5_C06Amount { get; set; }

        public string F5_C06AMetered { get; set; }

        public string F5_C06AAmount { get; set; }

        public string F5_C07Metered { get; set; }

        public string F5_C07Amount { get; set; }

        public string F5_C07AMetered { get; set; }

        public string F5_C07AAmount { get; set; }

        public string F5_C08Metered { get; set; }

        public string F5_C08Amount { get; set; }

        public string F5_C08AMetered { get; set; }

        public string F5_C08AAmount { get; set; }

        public string F5_C09Metered { get; set; }

        public string F5_C09Amount { get; set; }

        public string F5_C09AMetered { get; set; }

        public string F5_C09AAmount { get; set; }

        public string F5_C10Metered { get; set; }

        public string F5_C10Amount { get; set; }

        public string F5_C10AMetered { get; set; }

        public string F5_C10AAmount { get; set; }

        public string F5_C11Metered { get; set; }

        public string F5_C11Amount { get; set; }

        public string F5_C11AMetered { get; set; }

        public string F5_C11AAmount { get; set; }

        public string F5_C12Metered { get; set; }

        public string F5_C12Amount { get; set; }

        public string F5_C12AMetered { get; set; }

        public string F5_C12AAmount { get; set; }

        public string F5_C13Metered { get; set; }

        public string F5_C13Amount { get; set; }

        public string F5_C13AMetered { get; set; }

        public string F5_C13AAmount { get; set; }

        public string F5_C14Metered { get; set; }

        public string F5_C14Amount { get; set; }

        public string F5_C14AMetered { get; set; }

        public string F5_C14AAmount { get; set; }

        public string F5_C15Metered { get; set; }

        public string F5_C15Amount { get; set; }

        public string F5_C15AMetered { get; set; }

        public string F5_C15AAmount { get; set; }

        public string F5_C16Metered { get; set; }

        public string F5_C16Amount { get; set; }

        public string F5_C16AMetered { get; set; }

        public string F5_C16AAmount { get; set; }

        public string F5_C17Metered { get; set; }

        public string F5_C17Amount { get; set; }

        public string F5_C17AMetered { get; set; }

        public string F5_C17AAmount { get; set; }

        public string F5_C18Metered { get; set; }

        public string F5_C18Amount { get; set; }

        public string F5_C18AMetered { get; set; }

        public string F5_C18AAmount { get; set; }

        public string F5_C19Metered { get; set; }

        public string F5_C19Amount { get; set; }

        public string F5_C19AMetered { get; set; }

        public string F5_C19AAmount { get; set; }

        public string F5_C20Metered { get; set; }

        public string F5_C20Amount { get; set; }

        public string F5_C20AMetered { get; set; }

        public string F5_C20AAmount { get; set; }

        public string F5_C21Metered { get; set; }

        public string F5_C21Amount { get; set; }

        public string F5_C21AMetered { get; set; }

        public string F5_C21AAmount { get; set; }

    }
}
