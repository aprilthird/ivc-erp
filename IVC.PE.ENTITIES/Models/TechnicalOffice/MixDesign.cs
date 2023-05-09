using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class MixDesign
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }

        public Project Project { get; set; }
        public Guid DesignTypeId { get; set; }
        public DesignType DesignType { get; set; }
        public Guid CementTypeId { get; set; }
        public CementType CementType { get; set;}
        public Guid AggregateTypeId { get; set; }
        public AggregateType AggregateType { get; set; }
        public Guid ConcreteUseId { get; set; }
        public ConcreteUse ConcreteUse { get; set; }

        public bool Additive { get; set; }

        public string Name { get; set; }

        public DateTime DesignDate { get; set; }
        public Guid TechnicalVersionId { get; set; }

        public TechnicalVersion TechnicalVersion { get; set; }

        public Uri FileUrl { get; set; }
    }
}
