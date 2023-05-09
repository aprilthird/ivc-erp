using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice
{
    public class MixDesignResourceModel
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Guid CementTypeId { get; set; }

        public Guid AggregateTypeId { get; set; }

        public Guid ConcreteUseId { get; set; }

        public Guid DesignTypeId { get; set; }

        public string Date { get; set; }

        public Uri FileUrl { get; set; }

        public string CementDescription { get; set; }

        public string AggDescription { get; set; }

        public string ConcreteDescription { get; set; }

        public string DesignDescription { get; set; }
        public string Name { get; set; }
    }
}
