using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.IntegratedManagementSystem
{
    public class SewerManifoldFor24ThirdPart
    {
        public Guid Id { get; set; }
        public Guid SewerManifoldFor24SecondPartId { get; set; }
        public SewerManifoldFor24SecondPart SewerManifoldFor24SecondPart { get; set; }
        public int ActionTaken { get; set; }
        public bool PreventiveCorrectiveAction { get; set; }
        public DateTime ClosingDate { get; set; }
        public Uri FileUrl { get; set; }
    }
}
