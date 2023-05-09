using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.AdvancedSearchViewModels
{
    public class SearchResultViewModel
    {
        public Guid Id { get; set; }

        public int Type { get; set; }

        public Uri FileUrl { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime Date { get; set; }

        public string DateStr => Date.ToDateString();
    }
}
