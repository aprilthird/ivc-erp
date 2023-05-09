using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkerInvoiceSend
    {
        public Guid WorkerId { get; set; }
        public string Document { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string FullName => $"{PaternalSurname} {MaternalSurname} {Name}";
        public int Category { get; set; }
        public string CategoryDesc => ConstantHelpers.Worker.Category.VALUES[Category];
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public DateTime? DateSended { get; set; }
        public string DateSendedStr => DateSended.HasValue ? (DateSended.Value.ToDateString() + " " + DateSended.Value.ToTimeString()) : string.Empty;
        public string Observation { get; set; }
    }
}
