using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Dtos
{
    public class GenreDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
