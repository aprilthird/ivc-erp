using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.DocumentaryControl
{
    public class LetterInterestGroup
    {
        public Guid LetterId { get; set; }

        public Guid InterestGroupId { get; set; }

        public Letter Letter { get; set; }

        public InterestGroup InterestGroup { get; set; }
    }
}
