using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Warehouse.ViewModels.StockViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.StockRoofViewModels
{
    public class StockRoofViewModel
    {
        public Guid? Id { get; set; }

        public Guid StockId { get; set; }
        public StockViewModel Stock { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        public int RoofQuantity { get; set; }

        public Guid ProjectPhaseId { get; set; }
        public ProjectPhaseViewModel ProjectPhase { get; set; }
    }
}
