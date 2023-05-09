using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Biddings
{
	[NotMapped]
	public class UspProfessional
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Address { get; set; }
		public string University { get; set; }
		public string Nacionality { get; set; }
		public string MiddleName { get; set; }
		public string PaternalSurname { get; set; }
		public string MaternalSurname { get; set; }
		public string Document { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string CIPNumber { get; set; }
		public Guid ProfessionId { get; set; }
		public string ProfessionName { get; set; }
		public DateTime BirthDate { get; set; }

		public string BirthDateString => $"{BirthDate.ToDateString()}";
		public DateTime StartTitle { get; set; }
		public string StartTitleString => $"{StartTitle.ToDateString()}";
		public DateTime CipDate { get; set; }
		public string CipDateString => $"{CipDate.ToDateString()}";
		public bool ValidationSunedu { get; set; }
		public bool CertiAdult { get; set; }
		public Uri DniUrl { get; set; }
		public Uri TitleUrl { get; set; }
		public Uri CipUrl { get; set; }
		public Uri CertiAdultUrl { get; set; }
		public Uri CapacitationUrl { get; set; }

		public Guid? FoldingId {get; set;}

	public Guid? PositionId {get; set;}
}
}
