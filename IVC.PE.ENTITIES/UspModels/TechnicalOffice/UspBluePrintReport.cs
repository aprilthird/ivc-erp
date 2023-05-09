using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.TechnicalOffice
{
   public class UspBluePrintReport
    {
        public string Code { get; set; }
        public Guid? TechnicalVersionId { get; set; }

        public string TechnicalVersionDescription { get; set; }

        public Guid? LetterId { get; set; }

        public string LetterName { get; set; }

        public Guid? BudgetTitleId { get; set; }

        public string BudgetTitleName { get; set; }
        public Guid? ProjectFormulaId { get; set; }
        public string ProjectFormulaCode { get; set; }

        public string ProjectFormulaName { get; set; }

        public Guid? SpecialityId { get; set; }

        public string SpecialityDescription { get; set; }

        public string Name { get; set; }

        public Guid? WorkFrontId { get; set; }

        public string WorkFrontCode { get; set; }

        public Guid? ProjectId { get; set; }

        public string Description { get; set; }

        public string Sheet { get; set; }


        public Guid? ProjectPhaseId { get; set; }

        public string ProjectPhaseCode { get; set; }

        public string ProjectPhaseDescription { get; set; }

        public DateTime? BlueprintDate { get; set; }

        public Guid? BlueprintTypeId { get; set; }

        public string BlueprintTypeDescription { get; set; }

        public DateTime? DateType { get; set; }

        public string UserName { get; set; }

        public int Quantity { get; set; }

    }
}
