using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice
{
   public class BluePrintResourceModel
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string Code { get; set; }

        public string Sheet { get; set; }

        public string Description { get; set; }

        public Guid BudgetTitleId { get; set; }

        public string BudgetName { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public string ProjectFormulaCode { get; set; }

        public Guid SpecialityId { get; set; }
        public string SpecialityDescription { get; set; }

        public Guid WorkFrontId { get; set; }

        public string WorkFrontCode { get; set; }

        public string Name { get; set; }

        public string BlueprintDate { get; set; }

        public Guid TechnicalVersionId { get; set; }

        public string TechnicalVersionDescription { get; set; }

        public Uri FileUrl { get; set; }

        public Guid? LetterId { get; set; }

        public Uri LetterFileUrl { get; set; }
    }

    public class BluePrintListResourceModel
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string Color { get; set; }

        public string Code { get; set; }

        public string Sheet { get; set; }

        public string Description { get; set; }

        public Guid BudgetTitleId { get; set; }

        public string BudgetName { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public string ProjectFormulaCode { get; set; }

        public Guid SpecialityId { get; set; }
        public string SpecialityDescription { get; set; }

        public Guid WorkFrontId { get; set; }

        public string WorkFrontCode { get; set; }

        public string Name { get; set; }

        public string BlueprintDate { get; set; }

        public Guid TechnicalVersionId { get; set; }

        public string TechnicalVersionDescription { get; set; }

        public Uri FileUrl { get; set; }

        public Guid? LetterId { get; set; }

        public Uri LetterFileUrl { get; set; }

        public Guid BlueprintTypeId { get; set; }
    }

    public class BluePrintUspResourceModel
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public Uri FileUrl { get; set; }

        public Uri LetterUrl { get; set; }
    }
}
