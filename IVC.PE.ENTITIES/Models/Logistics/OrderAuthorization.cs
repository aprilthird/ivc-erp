using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class OrderAuthorization
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public string UserId { get; set; }
        public int UserType { get; set; }

        public bool IsApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}
