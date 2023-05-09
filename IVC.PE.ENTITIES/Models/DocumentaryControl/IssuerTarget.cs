using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.DocumentaryControl
{
    public class IssuerTarget
    {
        public Guid Id { get; set; }

        public string Acronym { get; set; }

        public string Name { get; set; }

        public Guid ProjectId { get; set; }

        public IEnumerable<LetterIssuerTarget> LetterIssuerTargets { get; set; }

        public IEnumerable<Letter> Letters { get; set; }
    }
}
