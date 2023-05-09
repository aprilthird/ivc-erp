using IVC.PE.ENTITIES.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkersInvoiceList
    {
        public Guid WorkerId { get; set; }
        public string PaternalName { get; set; }
        public string MaternalName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
