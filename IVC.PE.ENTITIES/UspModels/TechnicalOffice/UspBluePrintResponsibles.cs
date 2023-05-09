using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.TechnicalOffice
{
    [NotMapped]
    public class UspBluePrintResponsibles
    {
        public Guid ProjectId { get; set; }
        public Guid ProjectFormulaId { get; set; }
        public string ProjectAbbr { get; set; }
        public string UserNames { get; set; }
        public string FormulaName { get; set; }
        public string FormulaCode { get; set; }
    }
}
