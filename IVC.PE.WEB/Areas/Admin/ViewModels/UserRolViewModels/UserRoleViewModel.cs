using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.UserRolViewModels
{
    public class UserRoleViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public int Cantidad { get; set; }
    }
}
