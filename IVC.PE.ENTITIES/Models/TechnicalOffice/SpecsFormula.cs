using IVC.PE.ENTITIES.Models.General;
using System;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SpecsFormula
    {
        public Guid Id { get; set; }

        public Guid SpecialityId { get; set; }

        public Speciality Speciality { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }
    }
}
