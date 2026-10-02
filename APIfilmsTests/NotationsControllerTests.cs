using APIfilms.Controllers;
using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace APIfilmsTests
{
    [TestClass]
    public class NotationsControllerTests
    {
        [TestMethod]
        public void GetNotation_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            var notation = new Notation { UtilisateurId = 1, FilmId = 1, Note = 5 };
            var mockRepository = new Mock<IDataRepository<Notation>>();
            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new ActionResult<Notation>(notation));

            var controller = new NotationsController(mockRepository.Object);
            var actionResult = controller.GetNotation(1).Result;

            Assert.IsNotNull(actionResult);
            Assert.AreEqual(notation, actionResult.Value as Notation);
        }

        [TestMethod]
        public void GetNotation_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Notation>>();
            var controller = new NotationsController(mockRepository.Object);

            var actionResult = controller.GetNotation(0).Result;

            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostNotation_ModelValidated_CreationOK_AvecMoq()
        {
            var notation = new Notation { UtilisateurId = 2, FilmId = 3, Note = 4 };
            var mockRepository = new Mock<IDataRepository<Notation>>();
            var controller = new NotationsController(mockRepository.Object);

            var actionResult = controller.PostNotation(notation).Result;

            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult));
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result?.Value, typeof(Notation));
        }
    }
}