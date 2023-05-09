using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Biddings
{
    [NotMapped]
    public class UspProfessionalFirstExcel
    {

		public Guid Id { get; set; }
		public string Name {get; set;}
	public string PaternalSurname {get; set;}
	public string MaternalSurname {get; set;}
	public string Document {get; set;}
	public DateTime BirthDate {get; set;}

		public string BirthDateString => $"{BirthDate.ToDateString()}";
		public string Email {get; set;}
	public string PhoneNumber {get; set;}
	public Guid ProfessionId {get; set;}
	public string Profession {get; set;}
	public DateTime StartTitle {get; set;}
		public string StartTitleString => $"{StartTitle.ToDateString()}";
		public Uri TitleUrl {get; set;}
	public DateTime CipDate {get; set;}

		public string CipDateString => $"{CipDate.ToDateString()}";
		public string CIPNumber { get; set; }
	public Uri CipUrl {get; set;}

		public Guid BusinessId { get; set; }

		public string Business { get; set; }

		public Guid PositionId { get; set; }

		public string Position { get; set; }

		public bool ValidationSunedu { get; set; }
		public DateTime StartDate { get; set; }

		public string Address { get; set; }

		public string Nacionality { get; set; }

		public string University { get; set; }


		public string StartDateString => $"{StartDate.ToDateString()}";
	}
}
