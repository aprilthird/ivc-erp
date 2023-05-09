using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspFuelDates
    {
        public DateTime MondayDate { get; set; }

        public DateTime TuesdayDate { get; set; }

        public DateTime WednesdayDate { get; set; }

        public DateTime ThursdayDate { get; set; }

        public DateTime FridayDate { get; set; }

        public DateTime SaturdayDate { get; set; }

        public DateTime SundayDate { get; set; }

        public string MondayDateString => "- "+MondayDate.ToDateString();

        public string TuesdayDateString => "- "+TuesdayDate.ToDateString();

        public string WednesdayDateString => "- " + WednesdayDate.ToDateString();

        public string ThursdayDateString => "- " + ThursdayDate.ToDateString();

        public string FridayDateString => "- " + FridayDate.ToDateString();

        public string SaturdayDateString => "- " + SaturdayDate.ToDateString();

        public string SundayDateString => "- " + SundayDate.ToDateString();

        public string MondayDateString2 =>   MondayDate.ToDateString();

        public string TuesdayDateString2 =>  TuesdayDate.ToDateString();

        public string WednesdayDateString2 =>  WednesdayDate.ToDateString();

        public string ThursdayDateString2 =>   ThursdayDate.ToDateString();

        public string FridayDateString2 =>  FridayDate.ToDateString();

        public string SaturdayDateString2 => SaturdayDate.ToDateString();

        public string SundayDateString2 => SundayDate.ToDateString();
        public int FirstDay => MondayDate.Day;

        public int LastDay => SundayDate.Day;

        public int FirstMonth => MondayDate.Month;

        public int LastMonth => SundayDate.Month;

        public int Year => MondayDate.Year;
    }
}
