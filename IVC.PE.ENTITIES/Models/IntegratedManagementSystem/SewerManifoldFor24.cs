using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.IntegratedManagementSystem
{
    public class SewerManifoldFor24
    {
        public Guid Id { get; set; }
        public Guid SewerManifoldFor24FirstPartId { get; set; }
        public SewerManifoldFor24FirstPart SewerManifoldFor24FirstPart { get; set; }
        public Guid? SewerManifoldFor24SecondPartId { get; set; }
        public SewerManifoldFor24SecondPart SewerManifoldFor24SecondPart { get; set; }
        public Guid? SewerManifoldFor24ThirdpartId { get; set; }
        public SewerManifoldFor24ThirdPart SewerManifoldFor24ThirdPart { get; set; }
    }
}
