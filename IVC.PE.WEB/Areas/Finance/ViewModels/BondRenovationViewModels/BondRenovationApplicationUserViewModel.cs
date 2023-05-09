using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Finance.ViewModels.BondRenovationViewModels
{
    public class BondRenovationApplicationUserViewModel
    {
        public Guid? Id { get; set; }

        public Guid? BondRenovationId { get; set; }
        public BondRenovationViewModel BondRenovationViewModel { get; set; }

        public String UserId { get; set; }
    }
}
