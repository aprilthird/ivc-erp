using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class StockApplicationUser
    {
        public Guid Id { get; set; }
        public Guid StockId { get; set; }
        public Stock Stock { get; set; }
        public string UserId { get; set; }
    }
}
