using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationRenovationViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.LegalDocumentationTypeViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.ProfessionalsViewModels;
using IVC.PE.WEB.Areas.Bidding.ViewModels.SkillRenovationViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Bidding.ViewModels.SkillViewModels
{
    public class SkillViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Profesional", Prompt = "Profesional")]
        public Guid ProfessionalId { get; set; }

        public ProfessionalViewModel Professional { get; set; }

        public int NumberOfRenovations { get; set; }

        public SkillRenovationViewModel SkillRenovation{ get; set; }
        public IEnumerable<SkillRenovationViewModel> SkillRenovations { get; set; }
    }
}
