using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class GeneralExpense
    {
        public Guid Id { get; set; }

        public Guid BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        public int OrderNumber { get; set; }

        public string ItemNumber { get; set; }

        public string Description { get; set; }

        public double Quantity { get; set; }

        public string Unit { get; set; }
        
        public double Metered { get; set; }

        public double Price { get; set; }

        public double Parcial { get; set; }
    }
}
