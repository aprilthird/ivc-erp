namespace IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaSewerGroupViewModels
{   
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
    using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;

    public class ProjectFormulaSewerGroupViewModel
    {
        public Guid? Id { get; set; }
        public Guid SewerGroupId { get; set; }
        [Display(Name = "Cuadrillas", Prompt = "Cuadrillas")]
        public IEnumerable<Guid> SewerGroupIds { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }
        public Guid? ProjectFormulaId { get; set; }
        public ProjectFormulaViewModel ProjectFormula { get; set; }
    }
}
