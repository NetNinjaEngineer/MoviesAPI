using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAPI.Models
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }

        [MaxLength(2500)]
        public string Storeline { get; set; }

        public byte[] Poster { get; set; }

        public int GenreId { get; set; }

        public Genre Genre { get; set; }
    }
}