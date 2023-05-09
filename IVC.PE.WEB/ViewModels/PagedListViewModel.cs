using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.ViewModels
{
    public class PagedListViewModel<T>
    {
        public string Draw { get; set; } = ConstantHelpers.Datatable.ServerSide.SentParameters.DRAW_COUNTER;

        public int RecordsTotal { get; set; }

        public int RecordsFiltered { get; set; }

        public List<T> Data { get; set; }
    }
}
