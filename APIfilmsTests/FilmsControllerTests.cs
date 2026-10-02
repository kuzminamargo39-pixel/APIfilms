using APIfilms.Controllers;
using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace APIfilmsTests
{
    [TestClass]
    public class FilmsControllerTests
    {
        [TestMethod]
        public void GetFilm_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            var film = new Film { FilmId = 1, Titre = "Inception", Resume = "Dream within a dream" };
            var mockRepository = new Mock<IDataRepository<Film>>();
            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new ActionResult<Film>(film));

            var controller = new FilmsController(mockRepository.Object);
            var actionResult = controller.GetFilm(1).Result;

            Assert.IsNotNull(actionResult);
            Assert.AreEqual(film, actionResult.Value as Film);
        }

        [TestMethod]
        public void GetFilm_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Film>>();
            var controller = new FilmsController(mockRepository.Object);

            var actionResult = controller.GetFilm(0).Result;

            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostFilm_ModelValidated_CreationOK_AvecMoq()
        {
            var film = new Film { Titre = "Interstellar", Resume = "Space movie" };
            var mockRepository = new Mock<IDataRepository<Film>>();
            var controller = new FilmsController(mockRepository.Object);

            var actionResult = controller.PostFilm(film).Result;

            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult));
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result?.Value, typeof(Film));
        }

        [TestMethod]
        public void DeleteFilmTest_AvecMoq()
        {
            var film = new Film { FilmId = 1, Titre = "Inception" };
            var mockRepository = new Mock<IDataRepository<Film>>();
            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new ActionResult<Film>(film));

            var controller = new FilmsController(mockRepository.Object);
            var actionResult = controller.DeleteFilm(1).Result;

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
        }
    }
}