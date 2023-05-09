using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.ViewModels
{
    public class SelectViewModel<T>
    {
        public T Id { get; set; }

        public string Text { get; set; }
    }

    public class SelectViewModel : SelectViewModel<Guid>
    {
    }

    public class SelectIntViewModel : SelectViewModel<int>
    { }

    public class SelectStringViewModel : SelectViewModel<string>
    { }

}
