using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class LogisticResponsible
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public string UserId { get; set; }

        public int UserType { get; set; }
    }
}
