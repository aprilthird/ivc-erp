using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class WorkComponent
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public string Name { get; set; }

        public int PillColor { get; set; }
    }
}
