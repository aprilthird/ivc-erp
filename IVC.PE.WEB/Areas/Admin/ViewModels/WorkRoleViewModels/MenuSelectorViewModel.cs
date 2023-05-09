using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Admin.ViewModels.WorkRoleViewModels
{
    public class MenuSelectorViewModel
    {
        public List<AreaSelectorViewModel> Areas { get; set; }
    }

    public class AreaSelectorViewModel
    {
        public string Name { get; set; }

        public bool IsChecked { get; set; }

        public List<ItemSelectorViewModel> Items { get; set; }
    }

    public class ItemSelectorViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; }

        public int PermissionLevel { get; set; }

        public List<ItemSelectorViewModel> Items { get; set; }
    }
}
