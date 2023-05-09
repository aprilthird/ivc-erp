using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.ViewModels
{
    public class PaginationViewModel
    {
        public string Search { get; set; }

        public int RecordsPerPage { get; set; }

        public int CurrentNumber { get; set; }
    }
}