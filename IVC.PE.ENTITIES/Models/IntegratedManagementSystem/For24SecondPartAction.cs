using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.IntegratedManagementSystem
{
    public class For24SecondPartAction
    {
        public Guid Id { get; set; }
        public Guid SewerManifoldFor24SecondPartId {get; set; }
        public SewerManifoldFor24SecondPart SewerManifoldFor24SecondPart { get; set; }
        public string ActionName { get; set; }
        public DateTime Date { get; set; }
    }
}
