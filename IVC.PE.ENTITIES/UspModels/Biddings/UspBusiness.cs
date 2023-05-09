using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Biddings
{
    [NotMapped]
    public class UspBusiness
    {
        public Guid Id { get; set; }

        public int CodeNumber { get; set; }
        public string Code => $"EMP-{CodeNumber:D3}";

        public string BusinessName { get; set; }

        public string Tradename { get; set; }

        public string RUC { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }


        public string CollectionAreaContactName { get; set; }

        public string CollectionAreaEmail { get; set; }

        public string CollectionAreaPhoneNumber { get; set; }

        public Uri RucUrl { get; set; }

        public Uri TestimonyUrl { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateDateString => $"{CreateDate.ToDateString()}";

        public int Number { get; set; }

        public string LegalAgent { get; set; }

        public int Type { get; set; }
        //public DateTime? FromDate { get; set; }
        //public string FromDateString => FromDate.HasValue ? FromDate.Value.ToDateString() : string.Empty;
        //public DateTime? ToDate { get; set; }
        //public string ToDateString => ToDate.HasValue ? ToDate.Value.ToDateString() : string.Empty;


    }
}
