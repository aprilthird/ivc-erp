using IVC.PE.APP.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.APP.Infraestructure
{
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}
