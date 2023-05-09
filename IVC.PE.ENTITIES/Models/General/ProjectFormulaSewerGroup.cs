namespace IVC.PE.ENTITIES.Models.General
{
    using System;
    using IVC.PE.ENTITIES.Models.TechnicalOffice;

    public class ProjectFormulaSewerGroup
    {
        public Guid Id { get; set; }
        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormula ProjectFormula { get; set; }
    }
}
