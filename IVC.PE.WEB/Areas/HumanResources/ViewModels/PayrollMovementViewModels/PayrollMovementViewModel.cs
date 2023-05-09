using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementDetailViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollWorkerVariableViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementViewModels
{
    public class PayrollMovementViewModel
    {
        public string ProjectName { get; set; }
        public string WorkerName { get; set; }
        public string WorkerDocument { get; set; }
        public string CategoryName { get; set; }
        public string EntryDate { get; set; }
        public string CeaseDate { get; set; }
        public string FundAdministrator { get; set; }
        public string CUSSP { get; set; }
        public string MonthNameYear { get; set; }
        public string WeekRange { get; set; }
        public string WeekEnd { get; set; }
        public Dictionary<string, string> PayrollWorkerVariables { get; set; }
        public Dictionary<string, Concept> RemunerativeConcepts { get; set; }
        public Dictionary<string, Concept> DiscountConcepts { get; set; }
        public Dictionary<string, Concept> ContributiveConcepts { get; set; }
        public Dictionary<string, Concept> TotalConcepts { get; set; }
    }

    public class Concept
    {
        public string ShortDescription { get; set; }
        public string Value { get; set; }
    }
}
