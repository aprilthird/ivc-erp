using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class ProcedureFile
    {
        public Guid Id { get; set; }

        public Guid ProcedureId { get; set; }
        public Procedure Procedure { get; set; }

        public int Version { get; set; }

        public Uri FileUrl { get; set; }
    }
}
