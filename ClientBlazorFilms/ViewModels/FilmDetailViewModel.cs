using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ClientBlazorFilms.Models.DTO;
using ClientBlazorFilms.Services;

namespace ClientBlazorFilms.ViewModels;

public sealed partial class FilmDetailViewModel : ObservableObject
{
    private readonly IFilmService _filmService;

    public FilmDetailViewModel(IFilmService filmService)
    {
        _filmService = filmService;
    }

    [ObservableProperty]
    private FilmDetailDto? _film;

    [ObservableProperty]
    private string? _statusMessage;

    [ObservableProperty]
    private bool _isSavedSuccessfully;

    [RelayCommand]
    private async Task LoadDetail(int id, CancellationToken cancellationToken)
    {
        StatusMessage = "Chargement des détails...";
        try
        {
            Film = await _filmService.GetFilmByIdAsync(id, cancellationToken);
            StatusMessage = null;
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Chargement annulé.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur : {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task Save(CancellationToken cancellationToken)
    {
        if (Film == null) return;

        StatusMessage = "Sauvegarde en cours...";
        IsSavedSuccessfully = false;

        try
        {
            bool success = await _filmService.UpdateFilmAsync(Film, cancellationToken);
            if (success)
            {
                StatusMessage = "Film modifié avec succès !";
                IsSavedSuccessfully = true;
            }
            else
            {
                StatusMessage = "Échec de la modification du film.";
            }
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Sauvegarde annulée.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur : {ex.Message}";
        }
    }
}
