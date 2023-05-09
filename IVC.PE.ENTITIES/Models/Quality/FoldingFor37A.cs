using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class FoldingFor37A
    {
        public Guid Id { get; set; }
        public Guid SewerManifoldFor37AId { get; set; }
        public SewerManifoldFor37A SewerManifoldFor37A { get; set; }
        public int WeldingType { get; set; }
        public int MeetingNumber { get; set; }
        public DateTime Date { get; set; }
    }
}
