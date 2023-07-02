using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dtos;
using MoviesAPI.Models;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private long _maxAllowedPosterSize = 1048576;
        private List<string> _allowedExtensions = new() { ".jpg", ".png" };


        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _context.Movies
                .OrderByDescending(m => m.Rate)
                .Select(m=> new MovieDetailsDto
                {
                    GenreName = m.Genre.Name,
                    Id = m.Id,
                    GenreId = m.Genre.Id,
                    Poster = m.Poster,
                    Rate = m.Rate,
                    Title = m.Title,
                    Storeline = m.Storeline,
                    Year = m.Year
                })
                .ToListAsync();

            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Genre)
                .SingleOrDefaultAsync(x => x.Id == id);
            
            if (movie == null) 
                return NotFound(value: $"No Movie Found with id: {id}");

            var dto = new MovieDetailsDto
            {
                GenreId = movie.Genre.Id,
                Title = movie.Title,
                Storeline = movie.Storeline,
                Year = movie.Year,
                Rate = movie.Rate,
                Poster = movie.Poster,
                GenreName = movie.Genre.Name,
                Id = movie.Id
            };


            return Ok(dto);
        }

        [HttpGet("GetByGenreId")]
        public async Task<ActionResult<Movie>> GetByGenreIdAsync(int genreId)
        {
            var movies = await _context.Movies
                .Where(m => m.GenreId == genreId)
                .OrderByDescending(m => m.Rate)
                .Select(m => new MovieDetailsDto
                {
                    GenreName = m.Genre.Name,
                    Id = m.Id,
                    GenreId = m.Genre.Id,
                    Poster = m.Poster,
                    Rate = m.Rate,
                    Title = m.Title,
                    Storeline = m.Storeline,
                    Year = m.Year
                })
                .ToListAsync();

            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto movieDto)
        {
            if (!_allowedExtensions.Contains(Path.GetExtension(movieDto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");

            if (movieDto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("max allowed size for poster is 1 MB");

            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == movieDto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genre id!");

            using var dataStream = new MemoryStream();
            await movieDto.Poster.CopyToAsync(dataStream);

            var movie = new Movie
            {
                GenreId = movieDto.GenreId,
                Title = movieDto.Title,
                Rate = movieDto.Rate,
                Poster = dataStream.ToArray(),
                Storeline = movieDto.Storeline,
                Year = movieDto.Year
            };

            _context.Movies.Add(movie);

            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(x => x.Id == id);
            
            if (movie == null)
                return BadRequest($"No Movie Found with id: {id}");

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(x => x.Id == id);

            if (movie == null)
                return BadRequest($"No Movie Found with id: {id}");

            var isValidGenre = await _context.Genres.AnyAsync(g=> g.Id == dto.GenreId);
            
            if (!isValidGenre)
                return BadRequest("Invalid Genre Id !!!");

            if (dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed!");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("max allowed size for poster is 1 MB");

                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;
            movie.Rate = dto.Rate;
            movie.Storeline = dto.Storeline;
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;

            await _context.SaveChangesAsync();

            return Ok(movie);
        }
    }
}
