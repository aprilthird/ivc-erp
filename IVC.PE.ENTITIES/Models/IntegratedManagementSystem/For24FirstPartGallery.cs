using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.IntegratedManagementSystem
{
    public class For24FirstPartGallery
    {
        public Guid Id { get; set; }
        public Guid SewerManifoldFor24FirstPartId { get; set; }
        public SewerManifoldFor24FirstPart SewerManifoldFor24FirstPart { get; set; }
        public string Name { get; set; }
        public Uri URL { get; set; }
    }
}
