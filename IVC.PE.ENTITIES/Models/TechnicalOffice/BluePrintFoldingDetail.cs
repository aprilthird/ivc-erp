using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class BluePrintFoldingDetail
    { 
        public Guid Id { get; set; }

        public Guid BlueprintFoldingId { get; set; }

        public BlueprintFolding BlueprintFolding { get; set; }

        public DateTime DateType { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}
