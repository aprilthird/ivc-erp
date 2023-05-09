using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Biddings
{
    [NotMapped]
    public class UspBusinessResponsible
    {
        public Guid BusinessId { get; set; }

        public string BusinessName { get; set; }

        public string UserNames { get; set; }
    }
}
