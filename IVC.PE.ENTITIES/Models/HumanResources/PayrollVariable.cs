using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PayrollVariable
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public string Formula { get; set; }
    }
}
