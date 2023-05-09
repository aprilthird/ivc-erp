using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.APP.Common.Models
{
    public class SelectItem<T>
    {
        public T Id { get; set; }

        public string Text { get; set; }
    }

    public class SelectItem : SelectItem<Guid>
    { }

    public class SelectItemInt : SelectItem<int>
    { }
}
