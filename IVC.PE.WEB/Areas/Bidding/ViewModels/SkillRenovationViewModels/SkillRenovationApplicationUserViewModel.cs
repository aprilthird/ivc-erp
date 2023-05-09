using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.SkillRenovationViewModels
{
    public class SkillRenovationApplicationUserViewModel
    {
        public Guid? Id { get; set; }
        public Guid? SkillRenovationId { get; set; }
        public SkillRenovationViewModel SkillRenovation { get; set; }
        public String UserId { get; set; }
    }
}
