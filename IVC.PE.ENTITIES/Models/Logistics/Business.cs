using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class Business
    {
        public Guid Id { get; set; }

        public int CodeNumber { get; set; }
        
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

        public int Number { get; set; }

        public int Type { get; set; }

        public double? DefaultParticipation { get; set; } 

        public string BusinessConsortium1 { get; set; }
        public string BusinessConsortium2 { get; set; }
        public string BusinessConsortium3 { get; set; }
        public string BusinessConsortium4 { get; set; }
        public string BusinessConsortium5 { get; set; }

        public int NumberofParticipations { get; set; }


    }
}
