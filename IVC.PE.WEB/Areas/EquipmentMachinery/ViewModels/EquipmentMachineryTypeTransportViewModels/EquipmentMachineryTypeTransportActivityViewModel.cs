using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels
{
    public class EquipmentMachineryTypeTransportActivityViewModel
    {
        public Guid? Id { get; set; }

        public Guid? EquipmentMachineryTypeTransportId { get; set; }
        public EquipmentMachineryTypeTransportViewModel EquipmentMachineryTypeTransportViewModel { get; set; }
        public string Description { get; set; }
    }
}
