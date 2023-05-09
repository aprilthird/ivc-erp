using IVC.PE.ENTITIES.Models.DocumentaryControl;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class InterestGroup
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public IEnumerable<LetterInterestGroup> LetterInterestGroups { get; set; }

        public IEnumerable<ApplicationUserInterestGroup> UserInterestGroups { get; set; }
    }
}
