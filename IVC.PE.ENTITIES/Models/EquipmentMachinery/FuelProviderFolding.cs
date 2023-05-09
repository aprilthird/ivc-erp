using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.EquipmentMachinery
{
    public class FuelProviderFolding
    {
        public Guid Id { get; set; }

        public Guid FuelProviderId { get; set;}

    public FuelProvider FuelProvider { get; set; }

    public string CisternPlate {get; set;}


}
}
