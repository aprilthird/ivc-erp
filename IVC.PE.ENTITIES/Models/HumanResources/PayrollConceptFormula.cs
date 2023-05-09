using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PayrollConceptFormula
    {
        public Guid Id { get; set; }

        public Guid PayrollConceptId { get; set; }

        public PayrollConcept PayrollConcept { get; set; }

        public Guid? PayrollVariableId { get; set; }

        public PayrollVariable PayrollVariable { get; set; }

        public int LaborRegimeId { get; set; }

        public bool Active { get; set; }

        public string Formula { get; set; }

        public bool IsAffectedToEsSalud { get; set; }

        public bool IsAffectedToOnp { get; set; }

        public bool IsAffectedToQta { get; set; }

        public bool IsAffectedToAfp { get; set; }

        public bool IsAffectedToRetJud { get; set; }

        public bool IsComputableToCTS { get; set; }

        public bool IsComputableToGrati { get; set; }

        public bool IsComputableToVacac { get; set; }
    }
}
