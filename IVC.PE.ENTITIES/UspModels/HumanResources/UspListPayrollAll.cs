using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspListPayrollAll
    {
        public Guid WorkerId { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public int Category { get; set; }
        public string Document { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? CeaseDate { get; set; }
        public int Origin { get; set; }
        public int WorkGroup { get; set; }
        public string Position { get; set; }
        public string SewerGroupCode { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
    }
}
