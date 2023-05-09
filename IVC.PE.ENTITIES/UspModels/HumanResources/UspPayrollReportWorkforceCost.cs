using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspPayrollReportWorkforceCost
    {
        public Guid WorkerId { get; set; }
        public string Document { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string FullName => PaternalSurname + " " + MaternalSurname + " " + Name;
        public int Category { get; set; }
        public string CategoryName => ConstantHelpers.Worker.Category.VALUES[Category];
        public string Position { get; set; }
        public string ProjectPhase { get; set; }
        public string ProjectPhaseDescription { get; set; }
        public Guid SewerGroupId { get; set; }
        public string SewerGroup { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
    }
}
