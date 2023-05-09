using IVC.PE.ENTITIES.Models.DocumentaryControl;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SewerManifoldLetter
    {
        public Guid Id { get; set; }

        public Guid SewerManifoldId { get; set; }
        public SewerManifold SewerManifold { get; set; }

        public Guid LetterId { get; set; }
        public Letter Letter { get; set; }

        public int LetterType { get; set; }
    }
}
