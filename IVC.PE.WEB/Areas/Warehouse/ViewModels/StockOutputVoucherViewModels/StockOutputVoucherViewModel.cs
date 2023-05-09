using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Warehouse.ViewModels.StockOutputVoucherViewModels
{
    public class StockOutputVoucherViewModel
    {
        public Guid? Id { get; set; }

        public int VoucherType { get; set; }

        public string VoucherDate { get; set; }

        public Guid? SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        public Guid? ProjectPhaseId { get; set; }
        public ProjectPhaseViewModel ProjectPhase { get; set; }

        public string PickUpResponsible { get; set; }
        public string Observation { get; set; }

        public bool WasDelivered { get; set; }
    }
}
