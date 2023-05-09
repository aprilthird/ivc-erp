using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice
{
    public class FootageSewerBoxItemResourceModel
    {
        public Guid Id { get; set; }

        public Guid SewerBoxFootageId { get; set; }

        public int Group { get; set; }

        public int Type { get; set; }
        public string TypeStr { get; set; }

        public decimal RealFootage { get; set; }
        public decimal TechnicalRecordFootage { get; set; }
    }
}
