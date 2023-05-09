using IVC.PE.ENTITIES.Models.DocumentaryControl;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
   public class BlueprintFolding
    {
        public Guid Id { get; set; }

        public Guid BlueprintId { get; set; }
        public Blueprint Blueprint { get; set; }

        public string Code { get; set; }

        public Guid TechnicalVersionId { get; set; }

        public TechnicalVersion TechnicalVersion { get; set; }

        public Uri FileUrl { get; set; }

        public Uri CadUrl { get; set; }
        public Guid? LetterId { get; set; }

        public Letter Letter { get; set; }

        public DateTime? BlueprintDate { get; set; }
    }
}
