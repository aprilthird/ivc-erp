using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarWeekViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldCostPerformanceViewModels
{
    public class SewerManifoldCostPerformanceSewerGroupViewModel
    {
        public Guid? Id { get; set; }

        public Guid SewerManifoldCostPerformanceId { get; set; }
        public SewerManifoldCostPerformanceViewModel SewerManifoldCostPerformance { get; set; }

        public Guid ProjectCalendarWeekId { get; set; }
        public ProjectCalendarWeekViewModel ProjectCalendarWeek { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        public double WorkforceEquipment { get; set; }
        public double WorkforceEquipmentService { get; set; }

        public double SecurityFactor { get; set; }

        public double WorkforceEquipmentMinLength => WorkforceEquipment / SewerManifoldCostPerformance.WorkforceEquipmentSf;
        public double WorkforceEquipmentServiceMinLength => WorkforceEquipmentService / SewerManifoldCostPerformance.WorkforceEquipmentServicesSf;
    }

    public class SewerManifoldLoadSewerGroupCPViewModel
    {
        public double SecurityFactor { get; set; }
        public IFormFile File { get; set; }
    }
}
