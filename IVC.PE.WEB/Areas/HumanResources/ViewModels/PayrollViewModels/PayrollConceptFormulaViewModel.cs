using IVC.PE.CORE.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels
{
    public class PayrollConceptFormulaViewModel
    {
        public Guid? Id { get; set; }

        public Guid PayrollConceptId { get; set; }

        public PayrollConceptViewModel PayrollConcept { get; set; }

        public Guid? PayrollVariableId { get; set; }

        public PayrollVariableViewModel PayrollVariable { get; set; }

        [Display(Name = "Reg. Laboral", Prompt = "Reg. Laboral")]
        public int LaborRegimeId { get; set; }

        public string LaborRegimeName => ConstantHelpers.PayrollConceptFormula.LaborRegime.VALUES[LaborRegimeId];

        [Display(Name = "Activo", Prompt = "Activo")]
        public bool Active { get; set; } = true;

        [Display(Name = "Formula", Prompt = "Formula")]
        public string Formula { get; set; }

        [Display(Name = "¿Afecto EsSalud?", Prompt = "¿Afecto EsSalud?")]
        public bool IsAffectedToEsSalud { get; set; } = false;

        [Display(Name = "¿Afecto ONP?", Prompt = "¿Afecto ONP?")]
        public bool IsAffectedToOnp { get; set; } = false;

        [Display(Name = "¿Afecto 5ta Cat.?", Prompt = "¿Afecto 5ta Cat.?")]
        public bool IsAffectedToQta { get; set; } = false;

        [Display(Name = "¿Afecto AFP?", Prompt = "¿Afecto AFP?")]
        public bool IsAffectedToAfp { get; set; } = false;

        [Display(Name = "¿Afecto Ret. Judicial?", Prompt = "¿Afecto Ret. Judicial?")]
        public bool IsAffectedToRetJud { get; set; } = false;

        [Display(Name = "¿Afecto CTS?", Prompt = "¿Afecto CTS?")]
        public bool IsComputableToCTS { get; set; } = false;

        [Display(Name = "¿Afecto Gratificación?", Prompt = "¿Afecto Gratificación?")]
        public bool IsComputableToGrati { get; set; } = false;

        [Display(Name = "¿Afecto Vacaciones?", Prompt = "¿Afecto Vacaciones?")]
        public bool IsComputableToVacac { get; set; } = false;
    }
}
