using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.BINDINGRESOURCES.Areas.Warehouse
{
   public class TechoResourceModel
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string MeasurementUnitAbbreviation { get; set; }

        public string Metered { get; set; }
    }
}
