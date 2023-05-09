using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspMachValCustom
    {
        public int IdCount { get; set; }

        public Guid SewerGroupId { get; set; }

        public string SgCode { get; set; }

        public Guid MachineryPhaseId { get; set; }

        public string MpCode { get; set; }
        public Guid FatherId { get; set; }

        public double Acumulated { get; set; }

        public string AmmountFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", Acumulated);
    }
}
