using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class Process
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string ProcessName { get; set; }
        public string Code { get; set; }

        public Guid? ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
