using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollInvoiceViewModels
{
    public class PayrollInvoiceViewModel
    {
        public Guid WorkerId { get; set; }
        public string PaternalName { get; set; }
        public string MaternalName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
