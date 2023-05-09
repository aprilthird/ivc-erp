using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class StockVoucher
    {
        public Guid Id { get; set; }

        public int VoucherType { get; set; }

        public DateTime VoucherDate { get; set; }

        public string ReferencePurchaseOrder { get; set; }
        public string Supplier { get; set; }

        public Guid? SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        public Guid? ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }

        public string PickUpResponsible { get; set; }
        public string Observation { get; set; }

        public bool WasDelivered { get; set; }
    }
}
