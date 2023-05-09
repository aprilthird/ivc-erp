using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.IntegratedManagementSystem
{
    public class SewerManifoldFor24SecondPart
    {
        public Guid Id { get; set; }
        public Guid SewerManifoldFor24FirstPartId { get; set; }
        public SewerManifoldFor24FirstPart SewerManifoldFor24FirstPart { get; set; }
        public int Decision { get; set; }
        public string Other { get; set; }
        public int LaborerQuantity { get; set; }
        public double LaborerHoursMan { get; set; }
        public double LaborerTotalHoursMan { get; set; }
        public int OfficialQuantity { get; set; }
        public double OfficialHoursMan { get; set; }
        public double OfficialTotalHoursMan { get; set; }
        public int OperatorQuantity { get; set; }
        public double OperatorHoursMan { get; set; }
        public double OperatorTotalHoursMan { get; set; }
        public DateTime ProposedDate { get; set; }
        public Uri FileUrl { get; set; }
        public Uri VideoUrl { get; set; }
        public ICollection<For24SecondPartGallery> for24SecondPartGallery { get; set; }

    }
}
