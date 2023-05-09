using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.General
{
    public class WorkFrontHead
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public ApplicationUser User { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public Guid? ProjectId { get; set; }

        public Project Project { get; set; }

        public IEnumerable<SewerGroup> SewerGroups { get; set; }
    }
}
