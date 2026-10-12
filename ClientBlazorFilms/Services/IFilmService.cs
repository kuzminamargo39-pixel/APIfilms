using ClientBlazorFilms.Models.DTO;

namespace ClientBlazorFilms.Services;

public interface IFilmService
{
    Task<List<FilmDto>> GetFilmsAsync(CancellationToken cancellationToken = default);
    Task<FilmDetailDto?> GetFilmByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> UpdateFilmAsync(FilmDetailDto film, CancellationToken cancellationToken = default);

    Task<bool> CreateFilmAsync(FilmDetailDto film, CancellationToken cancellationToken = default);
}
