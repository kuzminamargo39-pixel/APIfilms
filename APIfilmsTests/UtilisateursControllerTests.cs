using APIfilms.Controllers;
using Moq;
using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using APIfilms.Models.DTO; // Додано для доступу до DTO
using AutoMapper;          // Додано для AutoMapper
using APIfilms.Profiles;   // Додано для доступу до твого профілю маплення

namespace APIfilmsTests
{
    [TestClass]
    public class UtilisateursControllerTests
    {
        // Допоміжний метод для швидкого створення маппера у тестах
        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<UtilisateurProfile>());
            return config.CreateMapper();
        }

        [TestMethod]
        public void PostUtilisateur_ModelValidated_CreationOK_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var mapper = GetMapper(); // Створюємо маппер
            var userController = new UtilisateursController(mockRepository.Object, mapper); // Додаємо маппер

            Utilisateur user = new Utilisateur
            {
                Nom = "POISSON",
                Prenom = "Pascal",
                Mobile = "0606060606",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                Cp = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France"
            };

            var actionResult = userController.PostUtilisateur(user).Result;

            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Utilisateur>), "Not an ActionResult<Utilisateur>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Not a CreatedAtActionResult");

            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result?.Value, typeof(Utilisateur), "Not a Utilisateur");

            user.UserId = ((Utilisateur)result!.Value!).UserId;
            Assert.AreEqual(user, (Utilisateur)result.Value, "Users not identical");
        }

        [TestMethod]
        public void GetUtilisateurById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            Utilisateur user = new Utilisateur
            {
                UserId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                Cp = "74200",
                Ville = "Allinges",
                Pays = "France"
            };

            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new ActionResult<Utilisateur>(user));
            var mapper = GetMapper();
            var userController = new UtilisateursController(mockRepository.Object, mapper); // Додаємо маппер

            var actionResult = userController.GetUtilisateurById(1).Result;

            // ЗМІНЕНО: Тепер контролер повертає OkObjectResult, а всередині лежить UtilisateurDetailDto
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);

            var dto = okResult.Value as UtilisateurDetailDto;
            Assert.IsNotNull(dto);
            Assert.AreEqual(user.Nom, dto.Nom); // Перевіряємо, що AutoMapper правильно переніс ім'я
            // Assert.AreEqual(user.Mail, dto.Mail); // Якщо ти виправила Email на Mail у DTO
        }

        [TestMethod]
        public void GetUtilisateurById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var mapper = GetMapper();
            var userController = new UtilisateursController(mockRepository.Object, mapper); // Додаємо маппер

            var actionResult = userController.GetUtilisateurById(0).Result;

            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetUtilisateurByEmail_ExistingEmailPassed_ReturnsRightItem_AvecMoq()
        {
            string testEmail = "clilleymd@last.fm";
            Utilisateur user = new Utilisateur { UserId = 1, Mail = testEmail, Pwd = "pwd", Nom = "Calida" };

            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByStringAsync(testEmail)).ReturnsAsync(new ActionResult<Utilisateur>(user));
            var mapper = GetMapper();
            var userController = new UtilisateursController(mockRepository.Object, mapper); // Додаємо маппер

            var actionResult = userController.GetUtilisateurByEmail(testEmail).Result;

            // ЗМІНЕНО: Тепер перевіряємо на OkObjectResult та UtilisateurDetailDto
            Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
            var okResult = actionResult.Result as OkObjectResult;

            var dto = okResult?.Value as UtilisateurDetailDto;
            Assert.IsNotNull(dto);
            Assert.AreEqual(user.Nom, dto.Nom);
        }

        [TestMethod]
        public void GetUtilisateurByEmail_UnknownEmailPassed_ReturnsNotFoundResult_AvecMoq()
        {
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var mapper = GetMapper();
            var userController = new UtilisateursController(mockRepository.Object, mapper); // Додаємо маппер

            var actionResult = userController.GetUtilisateurByEmail("unknown@test.com").Result;

            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteUtilisateurTest_AvecMoq()
        {
            Utilisateur user = new Utilisateur
            {
                UserId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!"
            };

            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new ActionResult<Utilisateur>(user));
            var mapper = GetMapper();
            var userController = new UtilisateursController(mockRepository.Object, mapper); // Додаємо маппер

            var actionResult = userController.DeleteUtilisateur(1).Result;

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Not a NoContentResult");
        }

        [TestMethod]
        public void PutUtilisateurTest_ValidUpdate_ReturnsNoContent_AvecMoq()
        {
            Utilisateur existingUser = new Utilisateur { UserId = 1, Nom = "OldName", Mail = "test@test.com", Pwd = "pwd" };
            Utilisateur updatedUser = new Utilisateur { UserId = 1, Nom = "NewName", Mail = "test@test.com", Pwd = "pwd" };

            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new ActionResult<Utilisateur>(existingUser));

            var mapper = GetMapper();
            var userController = new UtilisateursController(mockRepository.Object, mapper); // Додаємо маппер

            var actionResult = userController.PutUtilisateur(1, updatedUser).Result;

            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Not a NoContentResult");
        }
    }
}