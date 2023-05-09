using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.HumanResources
{
    [NotMapped]
    public class UspWorkersSunatTreg
    {
        public int DocumentType { get; set; }
        public string Document { get; set; }
        public DateTime BirthDate { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int AddressIndicator { get; set; }
        public string BankCode { get; set; }
        public string BankAccount { get; set; }
        public int LaborRegimen { get; set; }
        public string EducativeSituation { get; set; }
        public int SunatOcupation { get; set; }
        public int Disability { get; set; }
        public string PensionFundUniqueIdentificationCode { get; set; }
        public int SctrPensionType { get; set; }
        public string ContractType { get; set; }
        public int AlternativeRegimen { get; set; }
        public int MaxWorkTime { get; set; }
        public int NocturnalRegimen { get; set; }
        public int Sindicalize { get; set; }
        public int PaymentPeriodicity { get; set; }
        public int ActiveSituation { get; set; }
        public int ExemptIncome { get; set; }
        public int SpecialRegimen { get; set; }
        public int PaymentMethod { get; set; }
        public string OcupationalCategory { get; set; }
        public int DoubleTributation { get; set; }
        public int PeriodCategory { get; set; }
        public DateTime EntryDate { get; set; }
        public string HealthRegimen { get; set; }
        public string PensionFund { get; set; }
        public int SctrHealthType { get; set; }
        public string RucCompany { get; set; }
        public string EstablishmentCode { get; set; }
    }
}
