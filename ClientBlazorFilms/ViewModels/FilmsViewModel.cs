using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientBlazorFilms.Models.DTO;
using ClientBlazorFilms.Services;

namespace ClientBlazorFilms.ViewModels;

public sealed partial class FilmsViewModel : ObservableObject
{
    private readonly IFilmService _filmService;

    public FilmsViewModel(IFilmService filmService)
    {
        _filmService = filmService;
    }

    [ObservableProperty]
    private List<FilmDto> _films = new();

    [ObservableProperty]
    private string _searchTerm = string.Empty;

    [ObservableProperty]
    private string? _message;

    public IEnumerable<FilmDto> FilteredFilms =>
        string.IsNullOrWhiteSpace(SearchTerm)
            ? Films
            : Films.Where(f => (f.Titre?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                               (f.Genre?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false));

    partial void OnSearchTermChanged(string value)
    {
        OnPropertyChanged(nameof(FilteredFilms));
    }

    partial void OnFilmsChanged(List<FilmDto> value)
    {
        OnPropertyChanged(nameof(FilteredFilms));
    }

    [RelayCommand]
    private async Task LoadFilms(TimeSpan delay, CancellationToken cancellationToken)
    {
        Message = "Chargement en cours...";
        try
        {
            if (delay > TimeSpan.Zero)
            {
                await Task.Delay(delay, cancellationToken);
            }

            Films = await _filmService.GetFilmsAsync(cancellationToken);
            Message = null;
        }
        catch (OperationCanceledException)
        {
            Message = "Opération annulée par l'utilisateur.";
        }
        catch (Exception ex)
        {
            Message = $"Erreur : {ex.Message}";
        }
    }
}