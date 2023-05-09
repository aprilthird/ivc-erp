using IVC.PE.CORE.Helpers;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Biddings
{
    [NotMapped]
    public class UspLegalDocumentations
    {
        public Guid LegalDocumentationId { get; set; }

        public Guid BusinessId { get; set; }
        public string BusinessName { get; set; }

        public int BusinessCodeNumber { get; set; }
        public string BusinessCode => $"EMP-{BusinessCodeNumber:D3}"; 

        public string BusinessRuc { get; set; }

        public Guid LegalDocumentationTypeId { get; set; }
        
        public string LegalDocumentationType { get; set; }

        public Guid LegalDocumentationRenovationId { get; set; }

        public int LegalDocumentationOrder { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreateDateString => $"{CreateDate.ToDateString()}";
        public DateTime EndDate { get; set; }
        public string EndDateString => $"{EndDate.ToDateString()}";

        public int Validity { get; set; }
        public bool Days5 { get; set; }
        public bool IsTheLast { get; set; }
        public Uri FileUrl { get; set; }
    }
}
