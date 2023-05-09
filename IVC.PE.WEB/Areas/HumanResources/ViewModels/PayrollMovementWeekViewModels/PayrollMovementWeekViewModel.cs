using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollMovementWeekViewModels
{
    public class PayrollMovementWeekViewModel
    {
        public Guid WeekId { get; set; }

        public Guid WorkerId { get; set; }

        public string WorkerFullName { get; set; }

        public string SewerGroupCode { get; set; }

        //Variables Trabajador
        public string Wage { get; set; }
        public string Buc { get; set; }
        public string NumSons { get; set; }
        public string HasUnionFee { get; set; }
        public string HasSCTR { get; set; }
        public string JudRetAmmount { get; set; }
        public string JudRetPercent { get; set; }
        public string HasWeeklySettlement { get; set; }
        public string IsMixedCommission { get; set; }
        public string HasEsSaludPlusVida { get; set; }
        public string PensionFundCode { get; set; }
        public string MedicalLeaveDailyAllowance { get; set; }

        //Variables Tareo
        public string HoursNormal { get; set; }

        public string Hours60Percent { get; set; }

        public string Hours100Percent { get; set; }

        public string HoursMedicalRest { get; set; }

        public string HoursPaternityLeave { get; set; }

        public string HoursHoliday { get; set; }

        public string HoursPaidLeave { get; set; }

        //Variables Días
        public string UnPaidLeave { get; set; }
        public string LaborSuspension { get; set; }
        public string NonAttendance { get; set; }
        public string MedicalLeave { get; set; }
        public string AttendanceDays { get; set; }
    }
}
