using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerBoxFootageViewModels
{
    public class SewerBoxFootageItemViewModel
    {
        public Guid? Id { get; set; }

        public Guid SewerBoxFootageId { get; set; }
        public SewerBoxFootageViewModel SewerBoxFootage { get; set; }

        public int Group { get; set; }
        public string GroupStr => ConstantHelpers.Sewer.Box.Footage.Groups.VALUES[Group];

        public int Type { get; set; }
        public string TypeStr => ConstantHelpers.Sewer.Box.Footage.Types.VALUES[Type];

        public decimal RealFootage { get; set; }
        public decimal TechnicalRecordFootage { get; set; }
    }
}
