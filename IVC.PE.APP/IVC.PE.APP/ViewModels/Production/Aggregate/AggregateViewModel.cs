using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.APP.ViewModels.Production.Aggregate
{
    public class AggregateViewModel : BaseViewModel
    {
        public string AggregateUrl { get; set; }

        public AggregateViewModel()
        {
            this.AggregateUrl = "http://ivc-agregados.dynalias.com/";
        }
    }
}
