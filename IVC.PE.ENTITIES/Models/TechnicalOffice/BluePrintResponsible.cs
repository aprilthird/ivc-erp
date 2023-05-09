using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
   public class BluePrintResponsible
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid ProjectFormulaId { get; set; }
        public ProjectFormula ProjectFormula { get; set; }

        public string UserId { get; set; }
    }
}
