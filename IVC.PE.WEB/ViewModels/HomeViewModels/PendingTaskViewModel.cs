using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.ViewModels.HomeViewModels
{
    public class PendingTaskViewModel
    {
        public string Name { get; set; }
        
        public string Url { get; set; }

        public string DateTimeString => DateTime.ToLocalDateTimeFormat();

        public DateTime DateTime { get; set; }
    }
}
