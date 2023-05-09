using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollAuthorizationRequestViewModels
{
    public class PayrollAuthorizationRequestViewModel
    {
        public Guid Id { get; set; }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public Guid? WeekId { get; set; }
        public Guid? HeaderId { get; set; }

        public string TaskUserAuth1Id { get; set; }
        public string Responsible1FullName { get; set; }
        public bool WeeklyTaskAuth1 { get; set; }
        public bool UserAnswered1 { get; set; }

        public string TaskUserAuth2Id { get; set; }
        public string Responsible2FullName { get; set; }
        public bool WeeklyTaskAuth2 { get; set; }
        public bool UserAnswered2 { get; set; }

        public bool AlertsSent { get; set; }

        public string PayrollUserAuthId { get; set; }
        public string Responsible3FullName { get; set; }
        public bool WeeklyPayrollAuth { get; set; }
    }
}
