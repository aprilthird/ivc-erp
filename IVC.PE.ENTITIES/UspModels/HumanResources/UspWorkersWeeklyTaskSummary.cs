using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkersWeeklyTaskSummary
    {
        public bool IsHome { get; set; }
        public int Workers { get; set; }
        //Pawns
        public int Pawns { get; set; }
        public int PawnsP { get; set; }
        public int PawnsS { get; set; }
        //Officials
        public int Officials { get; set; }
        public int OfficialsP { get; set; }
        public int OfficialsS { get; set; }
        //Operators
        public int Operators { get; set; }
        public int OperatorsP { get; set; }
        public int OperatorsS { get; set; }

        public decimal WorkersHours { get; set; }
        //Pawns
        public decimal PawnsHours { get; set; }
        public decimal PawnsPHours { get; set; }
        public decimal PawnsSHours { get; set; }
        //Officials
        public decimal OfficialsHours { get; set; }
        public decimal OfficialsPHours { get; set; }
        public decimal OfficialsSHours { get; set; }
        //Operators
        public decimal OperatorsHours { get; set; }
        public decimal OperatorsPHours { get; set; }
        public decimal OperatorsSHours { get; set; }
    }
}
