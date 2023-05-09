using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
  public class InstructionalProcedure
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Code { get; set; }
        public DateTime? PublicationDate { get; set; }
        public Uri FileUrl { get; set; }
    }
}
