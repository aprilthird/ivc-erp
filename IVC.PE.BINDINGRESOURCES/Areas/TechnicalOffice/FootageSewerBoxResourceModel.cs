using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice
{
    public class FootageSewerBoxResourceModel
    {
        public Guid Id { get; set; }

        public int Type { get; set; }

        public int Range { get; set; }

        public IEnumerable<FootageSewerBoxItemResourceModel> SewerBoxFootageItems { get; set; }
    }
}
