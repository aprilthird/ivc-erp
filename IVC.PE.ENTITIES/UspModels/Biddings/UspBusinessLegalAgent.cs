using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Biddings
{
    [NotMapped]
    public class UspBusinessLegalAgent
    {
        public Guid Id { get; set; }

        public Guid BusinessId { get; set; }

        public bool IsActive { get; set; }

        public string LegalAgent { get; set; }

        public DateTime? FromDate { get; set; }
        public string FromDateString => FromDate.HasValue ? FromDate.Value.ToDateString() : string.Empty;
        public DateTime? ToDate { get; set; }
        public string ToDateString => ToDate.HasValue ? ToDate.Value.ToDateString() : string.Empty;


        public int Order { get; set; }

    }
}
