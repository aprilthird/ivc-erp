using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.Production.ViewModels.RdpItemViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Production.ViewModels.RdpReportViewModels
{
    public class RdpReportFullViewModel
    {
        public RdpReportViewModel RdpReportPartials { get; set; }
    }

    public class RdpReportViewModel
    {
        public Guid Id { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        public Guid ProjectPhaseId { get; set; }
        public ProjectPhaseViewModel ProjectPhase { get; set; }

        public DateTime ReportDate { get; set; }

        public List<RdpItemFootageViewModel> RdpItems { get; set; }
    }

    public class RdpReportAccumulatedViewModel
    {
        [Display(Name ="Fecha de los Acumulados", Prompt = "Fecha de los Acumulados")]
        public string ReportDate { get; set; }
        [Display(Name ="Archivo Excel", Prompt ="Archivo Excel")]
        public IFormFile File { get; set; }
    }
}
