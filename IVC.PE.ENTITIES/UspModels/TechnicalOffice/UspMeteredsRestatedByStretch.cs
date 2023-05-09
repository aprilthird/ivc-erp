using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.TechnicalOffice
{
    [NotMapped]
   public class UspMeteredsRestatedByStretch
    {
        public Guid Id { get; set; }
        public Guid BudgetTitleId { get; set; }
        
        public Guid ProjectFormulaId { get; set; }
        
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public double Metered { get; set; }
        public Guid? WorkFrontId { get; set; }
        public Guid? SewerGroupId { get; set; }
        
        public string WorkFrontCode { get; set; }

        public string SewerCode { get; set; }

        public Guid ProjectId { get; set; }


    }
}
