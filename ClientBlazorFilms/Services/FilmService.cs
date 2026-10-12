using System.Net.Http.Json;
using ClientBlazorFilms.Models.DTO;

namespace ClientBlazorFilms.Services;

public class FilmService : IFilmService
{
    private readonly HttpClient _httpClient;

    public FilmService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<FilmDto>> GetFilmsAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<List<FilmDto>>("api/films", cancellationToken)
               ?? new List<FilmDto>();
    }

    public async Task<FilmDetailDto?> GetFilmByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<FilmDetailDto>($"api/films/{id}", cancellationToken);
    }

    public async Task<bool> UpdateFilmAsync(FilmDetailDto film, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/films/{film.Id}", film, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateFilmAsync(FilmDetailDto film, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Films", film, cancellationToken);
        return response.IsSuccessStatusCode;
    }
}
