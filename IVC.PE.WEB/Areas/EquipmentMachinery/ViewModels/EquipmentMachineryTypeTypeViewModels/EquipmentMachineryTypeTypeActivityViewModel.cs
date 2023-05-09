using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTypeViewModels
{
    public class EquipmentMachineryTypeTypeActivityViewModel
    {
        public Guid? Id { get; set; }

        public Guid? EquipmentMachineryTypeTypeId { get; set;}
        public EquipmentMachineryTypeTypeViewModel EquipmentMachineryTypeTypeViewModel { get; set; }
        public string Description { get; set; }
    }
}
