using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.DocumentaryControl
{
    public class LetterIssuerTarget
    {
        public Guid LetterId { get; set; }

        public Guid IssuerTargetId { get; set; }

        public Letter Letter { get; set; }

        public IssuerTarget IssuerTarget { get; set; }
    }
}
