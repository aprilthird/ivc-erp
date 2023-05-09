using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PayrollWeekSummary
    {
        public Guid Id { get; set; }

        public Guid ProjectCalendarWeekId { get; set; }

        public ProjectCalendarWeek ProjectCalendarWeek { get; set; }

        //Chart HH
        public decimal TotalAllHours { get; set; }

        public decimal PawnTotalHours { get; set; }

        //public decimal PawnHomeTotalHours { get; set; }

        public decimal PawnHomeIVCHours { get; set; }

        public decimal PawnHomeSyndicateHours { get; set; }

        public decimal PawnHomePopulationHours { get; set; }

        //public decimal PawnCollaboratorTotalHours { get; set; }

        public decimal PawnCollaboratorCollaboratorHours { get; set; }

        public decimal PawnCollaboratorSyndicateHours { get; set; }

        public decimal PawnCollaboratorPopulationHours { get; set; }

        public decimal OfficialTotalHours { get; set; }

        //public decimal OfficialHomeTotalHours { get; set; }

        public decimal OfficialHomeIVCHours { get; set; }

        public decimal OfficialHomeSyndicateHours { get; set; }

        public decimal OfficialHomePopulationHours { get; set; }

        //public decimal OfficialCollaboratorTotalHours { get; set; }

        public decimal OfficialCollaboratorCollaboratorHours { get; set; }

        public decimal OfficialCollaboratorSyndicateHours { get; set; }

        public decimal OfficialCollaboratorPopulationHours { get; set; }

        public decimal OperatorTotalHours { get; set; }

        //public decimal OperatorHomeTotalHours { get; set; }

        public decimal OperatorHomeIVCHours { get; set; }

        public decimal OperatorHomeSyndicateHours { get; set; }

        public decimal OperatorHomePopulationHours { get; set; }

        //public decimal OperatorCollaboratorTotalHours { get; set; }

        public decimal OperatorCollaboratorCollaboratorHours { get; set; }

        public decimal OperatorCollaboratorSyndicateHours { get; set; }

        public decimal OperatorCollaboratorPopulationHours { get; set; }

        //Chart Monto
        public decimal TotalAllCosts { get; set; }

        public decimal PawnTotalCosts { get; set; }

        //public decimal PawnHomeTotalCosts { get; set; }

        public decimal PawnHomeIVCCosts { get; set; }

        public decimal PawnHomeSyndicateCosts { get; set; }

        public decimal PawnHomePopulationCosts { get; set; }

        //public decimal PawnCollaboratorTotalCosts { get; set; }

        public decimal PawnCollaboratorCollaboratorCosts { get; set; }

        public decimal PawnCollaboratorSyndicateCosts { get; set; }

        public decimal PawnCollaboratorPopulationCosts { get; set; }

        public decimal OfficialTotalCosts { get; set; }

        //public decimal OfficialHomeTotalCosts { get; set; }

        public decimal OfficialHomeIVCCosts { get; set; }

        public decimal OfficialHomeSyndicateCosts { get; set; }

        public decimal OfficialHomePopulationCosts { get; set; }

        //public decimal OfficialCollaboratorTotalCosts { get; set; }

        public decimal OfficialCollaboratorCollaboratorCosts { get; set; }

        public decimal OfficialCollaboratorSyndicateCosts { get; set; }

        public decimal OfficialCollaboratorPopulationCosts { get; set; }

        public decimal OperatorTotalCosts { get; set; }

        //public decimal OperatorHomeTotalCosts { get; set; }

        public decimal OperatorHomeIVCCosts { get; set; }

        public decimal OperatorHomeSyndicateCosts { get; set; }

        public decimal OperatorHomePopulationCosts { get; set; }

        //public decimal OperatorCollaboratorTotalCosts { get; set; }

        public decimal OperatorCollaboratorCollaboratorCosts { get; set; }

        public decimal OperatorCollaboratorSyndicateCosts { get; set; }

        public decimal OperatorCollaboratorPopulationCosts { get; set; }
    }
}
