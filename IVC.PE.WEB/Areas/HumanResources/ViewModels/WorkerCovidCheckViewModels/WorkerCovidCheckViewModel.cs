using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerCovidCheckViewModels
{
    public class WorkerCovidCheckViewModel
    {
        public Guid? Id { get; set; }

        public Guid? WorkerId { get; set; }

        public WorkerViewModel Worker { get; set; }

        public string Document { get; set; }

        [Display(Name ="Fecha Prueba", Prompt = "Fecha Prueba")]
        public string CheckDate { get; set; }

        [Display(Name = "Tipo de Prueba", Prompt = "Tipo de Prueba")]
        public int TestType { get; set; }
        public string TestTypeDescription => ConstantHelpers.CovidTest.Type.VALUES[TestType];

        [Display(Name = "IgM", Prompt = "IgM")]
        public int? IgM { get; set; }
        public string IgMDescription => IgM.HasValue ? ConstantHelpers.CovidTest.Outcome.VALUES[IgM.Value] : "---";

        [Display(Name = "IgG", Prompt = "IgG")]
        public int? IgG { get; set; }
        public string IgGDescription => IgG.HasValue ? ConstantHelpers.CovidTest.Outcome.VALUES[IgG.Value] : "---";

        [Display(Name = "Resultado", Prompt = "Resultado")]
        public int? TestOutcome { get; set; }
        public string TestOutcomeDescription => TestOutcome.HasValue ? ConstantHelpers.CovidTest.Outcome.VALUES[TestOutcome.Value] : "---";

        public Uri FileUrl { get; set; }
    }

    public class WorkerCovidListViewModel
    {
        public Guid WorkerId { get; set; }
        public string Document { get; set; }
        public string FullName { get; set; }
        public bool Status { get; set; }
        public WorkerCovidCheckViewModel WorkerCovid { get; set; }
    }
}
