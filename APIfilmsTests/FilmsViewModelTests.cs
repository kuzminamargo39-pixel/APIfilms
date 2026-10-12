using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ClientBlazorFilms.ViewModels;
using ClientBlazorFilms.Services;
using ClientBlazorFilms.Models.DTO;

namespace APIfilmsTests;

[TestClass]
public class FilmsViewModelTests
{
    private Mock<IFilmService> _mockFilmService = null!;
    private FilmsViewModel _viewModel = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockFilmService = new Mock<IFilmService>();
        _viewModel = new FilmsViewModel(_mockFilmService.Object);
    }

    [TestMethod]
    public async Task LoadFilmsCommand_ShouldPopulateFilmsList_WhenServiceReturnsData()
    {
        var mockFilms = new List<FilmDto>
        {
            new() { Id = 1, Titre = "Inception", Genre = "Sci-Fi" },
            new() { Id = 2, Titre = "Interstellar", Genre = "Sci-Fi" }
        };

        _mockFilmService
            .Setup(s => s.GetFilmsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockFilms);

        await _viewModel.LoadFilmsCommand.ExecuteAsync(TimeSpan.Zero);

        Assert.IsNotNull(_viewModel.Films);
        Assert.AreEqual(2, _viewModel.Films.Count);
        Assert.AreEqual("Inception", _viewModel.Films[0].Titre);
        Assert.IsNull(_viewModel.Message);
        _mockFilmService.Verify(s => s.GetFilmsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task LoadFilmsCommand_ShouldHandleCancellation()
    {
        _mockFilmService
            .Setup(s => s.GetFilmsAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        await _viewModel.LoadFilmsCommand.ExecuteAsync(TimeSpan.Zero);

        Assert.IsNotNull(_viewModel.Message);
        StringAssert.Contains(_viewModel.Message, "annulée");
    }
}