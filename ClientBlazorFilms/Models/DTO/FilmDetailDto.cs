using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClientBlazorFilms.Models.DTO;

public class FilmDetailDto
{
    [JsonPropertyName("filmId")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le titre du film est obligatoire.")]
    [StringLength(150, ErrorMessage = "Le titre ne doit pas dépasser 150 caractères.")]
    [JsonPropertyName("titre")]
    public string Titre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le résumé est obligatoire.")]
    [JsonPropertyName("resume")]
    public string? Resume { get; set; }

    [Required(ErrorMessage = "La date de sortie est obligatoire.")]
    [JsonPropertyName("dateSortie")]
    public DateTime? DateSortie { get; set; } = DateTime.Today;

    [Range(1, 600, ErrorMessage = "La durée doit être comprise entre 1 et 600 minutes.")]
    [JsonPropertyName("duree")]
    public int Duree { get; set; }

    [Required(ErrorMessage = "Le genre est obligatoire.")]
    [JsonPropertyName("genre")]
    public string Genre { get; set; } = string.Empty;

    public bool EstRecent => DateSortie.HasValue && DateSortie.Value.Year >= DateTime.Now.Year - 2;

    public string DureeFormatee => $"{Duree / 60}h {Duree % 60}m";
}