using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class Bank
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string SunatCode { get; set; }

        //public IEnumerable<Provider> Providers { get; set; }

        //public IEnumerable<Provider> TaxProviders { get; set; }
    }
}
