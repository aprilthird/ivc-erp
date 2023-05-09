using IVC.PE.ENTITIES.UspModels.HumanResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementViewModels
{
    public class WorkerInvoiceViewModel
    {
        public UspWorkerInvoiceWokerInfo WorkerInfo { get; set; }
        public IEnumerable<UspWorkerInvoiceConcept> IncomeConcepts { get; set; }
        public IEnumerable<UspWorkerInvoiceConcept> DiscountConcepts { get; set; }
        public IEnumerable<UspWorkerInvoiceConcept> ContributionConcepts { get; set; }
        public IEnumerable<UspWorkerInvoiceConcept> TotalConcepts { get; set; }
        public IEnumerable<UspWorkerInvoiceConcept> DailyConcepts { get; set; }
        public string MonthYear { get; set; }
        public string WeekRange { get; set; }
    }
}
