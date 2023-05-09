using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerBoxFootageViewModels
{
    public class SewerBoxFootageViewModel
    {
        public Guid Id { get; set; }

        public int Type { get; set; }
        public string TypteStr => ConstantHelpers.Sewer.Box.Type.VALUES[Type];

        public int Range { get; set; }
        public string RangeStr => ConstantHelpers.Sewer.Box.Footage.Ranges.VALUES[Range];
    }
}
