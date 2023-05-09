using IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTypeTransportViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.EquipmentMachinery.ViewModels.EquipmentMachineryTransportPartViewModels
{
    public class EquipmentMachineryTransportPartPlusViewModel
    {

        public Guid? Id { get; set; }

        public Guid EquipmentMachineryTransportPartFoldingId { get; set; }

        public EquipmentMachineryTransportPartFoldingViewModel EquipmentMachineryTransportPartFolding { get; set; }

        public Guid EquipmentMachineryTypeTransportActivityId { get; set; }

        public EquipmentMachineryTypeTransportActivityViewModel EquipmentMachineryTypeTransportActivity { get; set; }

        public string Specific { get; set; }


    }
}
