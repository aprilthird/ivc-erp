using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class ReEntryForReturn
    {
        public Guid Id { get; set; }

        public int DocumentNumber { get; set; }
        
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormula ProjectFormula { get; set; }

        public Guid WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        //public Guid WarehouseId { get; set; }
        //public Warehouse Warehouse { get; set; }

        public Guid SupplyFamilyId { get; set; }
        public SupplyFamily SupplyFamily { get; set; }

        public DateTime ReturnDate { get; set; }

        public string UserId { get; set; }

        public Uri FileUrl { get; set; }

        public int Status { get; set; }
    }
}
