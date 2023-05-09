using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.TechnicalOffice
{
   public class UspSpecFormula
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string Description { get; set; }

        public string Formulas { get; set; }
    }
}
