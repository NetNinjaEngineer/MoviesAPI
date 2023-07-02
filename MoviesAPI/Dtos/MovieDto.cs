using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dtos
{
    public class MovieDto
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string Storeline { get; set; }
        public IFormFile Poster { get; set; }
        public int GenreId { get; set; }
    }
}