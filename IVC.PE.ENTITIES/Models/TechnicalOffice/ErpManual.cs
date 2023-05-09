using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class ErpManual
    {
        public Guid Id { get; set; }

        public Guid AreaModuleId {get; set;}

        public AreaModule AreaModule { get; set; }

        public string Name { get; set; }

        public Uri FileUrl { get; set; }
}
}
