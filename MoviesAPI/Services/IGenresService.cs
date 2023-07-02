using MoviesAPI.Models;

namespace MoviesAPI.Services
{
    public interface IGenresService
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> GetById(int id);
        Task<Genre> Add(Genre genre);
        Task<Genre> Update(Genre genre);
        Task<Genre> Delete(Genre genre);
    }
}
