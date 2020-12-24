using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArkadiuszGrygorukZadanie.Models
{
    public class Move
    {
        [Key]
        public int Id { get; set; }

        public string IMDbId { get; set; }

        [Required]
        public string Title { get; set; }

        [Range(1,10)]
        public float Rating { get; set; }

        public string ImageUrl { get; set; }

        public string ReleaseDate { get; set; }

        public string Description { get; set; }
    }
}