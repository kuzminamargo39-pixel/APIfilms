using System.Text.Json.Serialization;

namespace ClientBlazorFilms.Models.DTO;

public class FilmDto
{
    [JsonPropertyName("filmId")]
    public int Id { get; set; }

    [JsonPropertyName("titre")]
    public string Titre { get; set; } = string.Empty;

    [JsonPropertyName("dateSortie")]
    public DateTime? DateSortie { get; set; }

    [JsonPropertyName("genre")]
    public string? Genre { get; set; }

    public int? AnneeSortie => DateSortie?.Year;
}