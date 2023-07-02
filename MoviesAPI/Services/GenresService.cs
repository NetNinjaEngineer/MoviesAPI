using Microsoft.EntityFrameworkCore;
using MoviesAPI.Models;

namespace MoviesAPI.Services
{
    public class GenresService : IGenresService
    {
        private readonly AppDbContext _context;
        public GenresService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> Add(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<Genre> Delete(Genre genre)
        {
            _context.Genres.Remove(genre);

            await _context.SaveChangesAsync();

            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<Genre> GetById(int id)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);

            return genre!;
        }

        public async Task<Genre> Update(Genre genre)
        {
            _context.Update(genre);
            await _context.SaveChangesAsync();
            return genre;
        }
    }
}
