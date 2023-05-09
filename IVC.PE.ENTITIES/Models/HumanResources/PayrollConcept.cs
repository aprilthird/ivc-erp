using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PayrollConcept
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public int CategoryId { get; set; }

        public string SunatCode { get; set; }
    }
}
