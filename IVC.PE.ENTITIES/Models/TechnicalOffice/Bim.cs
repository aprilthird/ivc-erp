using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class Bim
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }

        public Project Project { get; set; }
        public Guid ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }

        public Guid WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; } 

        public Uri FileUrl { get; set; }

        public string Name { get; set; }
    }
}
