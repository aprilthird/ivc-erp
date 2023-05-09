using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldCostPerformanceViewModels
{
    public class SewerManifoldCostPerformanceViewModel
    {
        public Guid? Id { get; set; }

        //public Guid ProjectId { get; set; }
        //public ProjectViewModel Project { get; set; }

        public string Description { get; set; }

        public int TerrainType { get; set; }
        public string TerrainTypeStr => ConstantHelpers.Terrain.Type.VALUES[TerrainType];

        public double HeightMin { get; set; }
        public double HeightMax { get; set; }

        public string Unit { get; set; }

        public double Price { get; set; }

        public double Workforce { get; set; }
        public double Equipment { get; set; }
        public double Services { get; set; }
        public double Materials { get; set; }

        public double SecurityFactor { get; set; }

        public double WorkforceEquipment => Workforce + Equipment;
        public double WorkforceEquipmentServices => WorkforceEquipment + Services;

        public double WorkforceEquipmentSf => WorkforceEquipment * (1 - SecurityFactor / 100.0);
        public double WorkforceEquipmentServicesSf => WorkforceEquipmentServices * (1 - SecurityFactor / 100.0);
    }

    public class SewerManifoldLoadCPViewModel
    {
        public double SecurityFactor { get; set; }
        public IFormFile File { get; set; }
    }
}
