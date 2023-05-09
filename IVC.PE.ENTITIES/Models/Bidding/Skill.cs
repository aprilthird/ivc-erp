using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
    public class Skill
    {
        public Guid Id { get; set; }

        public Guid ProfessionalId { get; set; }

        public Professional Professional { get; set; }

        public int NumberOfRenovations { get; set; }
    }
}
