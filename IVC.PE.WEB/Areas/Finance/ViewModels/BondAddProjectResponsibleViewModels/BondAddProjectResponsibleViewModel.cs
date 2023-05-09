using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondGuarantorViewModels;
using IVC.PE.WEB.Areas.Finance.ViewModels.BondTypeViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.BankViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Finance.ViewModels.BondAddProjectResponsibleViewModels
{
    public class BondAddProjectResponsibleViewModel
    {    
        [Display(Name = "Proyectos", Prompt = "Proyectos")]
        public Guid ProjectId { get; set; }
        public ProjectViewModel Project { get; set; }

        [Display(Name = "Encargado(s)", Prompt = "Encargado(s)")]
        public IEnumerable<String> Responsibles { get; set; }
    }
}
