using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.ViewModels
{
    public class ChartViewModel
    {
        public List<Col> cols { get; set; }
        public List<Row> rows { get; set; }

        public class Col
        {
            public string label { get; set; }
            public string type { get; set; }

            public string role { get; set; }
        }

        public class Row
        {
            public List<C> c { get; set; }
        }

        public class C
        {
            public object v { get; set; }
        }
    }
}
