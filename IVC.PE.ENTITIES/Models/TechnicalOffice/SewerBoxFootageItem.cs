using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SewerBoxFootageItem
    {
        public Guid Id { get; set; }

        public Guid SewerBoxFootageId { get; set; }
        public SewerBoxFootage SewerBoxFootage { get; set; }

        public int Group { get; set; }

        public int Type { get; set; }
        public string TypeStr => ConstantHelpers.Sewer.Box.Footage.Types.VALUES[Type];

        public decimal RealFootage { get; set; }
        public decimal TechnicalRecordFootage { get; set; }
    }
}
