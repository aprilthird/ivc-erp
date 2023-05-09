using IVC.PE.WEB.Areas.Logistics.ViewModels.RequestViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Logistics.ViewModels.OrderViewModels
{
    public class RequestsInOrderViewModel
    {
        public Guid? Id { get; set; }
        public Guid RequestId { get; set; }
        public RequestViewModel Request { get; set; }
    }
}
