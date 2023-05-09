using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice
{
    public class ErpManualResourceModel
    {
        public Guid Id { get; set; }

        public Guid AreaModuleId {get; set;}

        public string AreaModuleDescription { get; set; }

        public string Name { get; set; }

        public Uri FileUrl { get; set; }


    }
}
