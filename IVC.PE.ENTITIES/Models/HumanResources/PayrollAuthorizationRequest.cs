using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PayrollAuthorizationRequest
    {
        public Guid Id { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public Guid WeekId { get; set; }

        public string TaskUserAuth1Id { get; set; }
        public bool WeeklyTaskAuth1 { get; set; }
        public bool UserAnswered1 { get; set; }
        public string TaskUserAuth2Id { get; set; }
        public bool WeeklyTaskAuth2 { get; set; }
        public bool UserAnswered2 { get; set; }

        public string PayrollUserAuthId { get; set; }
        public bool WeeklyPayrollAuth { get; set; }
        public bool PayrollAuthRequested { get; set; }
        public bool PayrollAuthAnswered { get; set; }
        public bool IsPayrollOk { get; set; }
    }
}
