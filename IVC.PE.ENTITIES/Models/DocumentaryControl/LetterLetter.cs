using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.DocumentaryControl
{
    public class LetterLetter
    {
        public Guid LetterId { get; set; }

        public Letter Letter { get; set; }

        public Guid ReferenceId { get; set; }
        
        public Letter Reference { get; set; }
    }
}
