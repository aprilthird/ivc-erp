using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class ProcessTypeDocument
    {
        public Guid Id { get; set; }

        public Guid ProcessId { get; set; }

        public Process Process { get; set; }

        public string DocumentType {get; set;}

        public string Code { get; set; }


    }
}
