using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class FieldRequestProjectFormula
    {
        public Guid Id { get; set; }

        public Guid FieldRequestId { get; set; }

        public FieldRequest FieldRequest { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }
    }
}
