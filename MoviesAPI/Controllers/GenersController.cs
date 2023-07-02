using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Dtos;
using MoviesAPI.Models;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenersController : ControllerBase
    {
        private readonly IGenresService _genresService;

        public GenersController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var geners = await _genresService.GetAll();
            
            return Ok(geners);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var genre = await _genresService.GetById(id);

            if (genre == null)
                return BadRequest($"No genre with id: {id}");

            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto genreDto)
        {
            var genre = new Genre { Name =  genreDto.Name };
            
            await _genresService.Add(genre);

            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] GenreDto genreDto)
        {
            var genre = await _genresService.GetById(id);

            if (genre == null)
               return NotFound($"No genre was found with ID: {id}");

            genre.Name = genreDto.Name;

            await _genresService.Update(genre);

            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genre = await _genresService.GetById(id);

            if (genre == null)
                return NotFound($"No genre was found with id: {id}");

            await _genresService.Delete(genre);

            return Ok(genre);
        }
    }
}