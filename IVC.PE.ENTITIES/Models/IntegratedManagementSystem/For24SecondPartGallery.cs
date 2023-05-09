using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.IntegratedManagementSystem
{
    public class For24SecondPartGallery
    {
        public Guid Id { get; set; }
        public Guid SewerManifoldFor24SecondPartId { get; set; }
        public SewerManifoldFor24SecondPart SewerManifoldFor24SecondPart { get; set; }
        public string Name { get; set; }
        public Uri URL { get; set; }
    }
}
