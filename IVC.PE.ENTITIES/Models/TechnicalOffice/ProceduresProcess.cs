using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
   public class ProceduresProcess
    {
        public Guid Id { get; set; }

        public Guid ProcedureId { get; set; }

        public Procedure Procedure { get; set; }

        public Guid ProcessId { get; set; }
        public Process Process { get; set; }


    }
}
