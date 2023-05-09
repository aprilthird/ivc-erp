using IVC.PE.CORE.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Biddings
{
    [NotMapped]
    public class UspSkills
    {
        public Guid SkillId { get; set; }

        public Guid SkillRenovationId { get; set; }

        public int SkillOrder { get; set; }

        public int Validity { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreateDateString => $"{CreateDate.ToDateString()}";
        public DateTime EndDate { get; set; }
        public string EndDateString => $"{EndDate.ToDateString()}";

        public bool Days15 { get; set; }

        public bool Days30 { get; set; }
        public bool IsTheLast { get; set; }
        public Uri FileUrl { get; set; }

        public Guid ProfessionalId { get; set; }

        public string ProfessionalName { get; set; }

        public string Document { get; set; }

        public string CIPNumber { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Guid ProfessionId { get; set; }

        public string Profession { get; set; }

    }
}
