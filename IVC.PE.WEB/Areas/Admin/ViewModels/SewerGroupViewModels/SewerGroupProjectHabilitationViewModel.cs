using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels
{
    public class SewerGroupProjectHabilitationViewModel
    {
        public Guid? Id { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        public Guid ProjectHabilitationId { get; set; }
        public ProjectHabilitationViewModel ProjectHabilitation { get; set; }
        [Display(Name ="Habilitaciones", Prompt ="Habilitaciones")]
        public IEnumerable<Guid> ProjectHabilitationIds { get; set; }

    }
}
