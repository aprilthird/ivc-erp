using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.ViewModels
{
    public class MenuViewModel
    {
        public string Name { get; set; }

        public string Area { get; set; }

        public List<MenuItemViewModel> Items { get; set; }
    }

    public class MenuItemViewModel
    {
        public string Name { get; set; }

        public string Controller { get; set; }        
        
        public string Action { get; set; }

        public bool IsGrouping { get; set; }

        public List<MenuItemViewModel> Items { get; set; }
    }
}
