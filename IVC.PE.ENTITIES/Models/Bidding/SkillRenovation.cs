using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
    public class SkillRenovation
    {
        public Guid Id { get; set; }

        public int SkillOrder { get; set; }

        public Guid SkillId { get; set; }

        public Skill Skill { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public DateTime EndDate { get; set; } = DateTime.UtcNow;

        public bool Days15 { get; set; } = false;

        public bool Days30 { get; set; } = false;

        public Uri FileUrl { get; set; }

        public bool IsTheLast { get; set; }
    }
}
