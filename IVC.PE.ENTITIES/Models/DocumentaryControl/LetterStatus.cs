using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.DocumentaryControl
{
    public class LetterStatus
    {
        public Guid LetterId { get; set; }

        public Letter Letter { get; set; }

        public int Status { get; set; }

        public Guid LetterDocumentCharacteristicId { get; set; }
    }
}
