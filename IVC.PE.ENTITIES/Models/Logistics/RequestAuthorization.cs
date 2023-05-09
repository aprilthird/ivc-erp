using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class RequestAuthorization
    {
        public Guid Id { get; set; }

        public Guid RequestId { get; set; }
        public Request Request { get; set; }

        public string UserId { get; set; }
        public int UserType { get; set; }

        public bool IsApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }
}
