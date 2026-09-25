using APIfilms.Controllers;
using APIfilms.Models.DataManager;
using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace APIfilmsTests
{
    [TestClass]
    public class UtilisateursControllerTests
    {
        private readonly FilmRatingsDBContext _context;
        private readonly UtilisateursController _controller;
        private readonly IDataRepository<Utilisateur> dataRepository;

        public UtilisateursControllerTests()
        {
            var builder = new DbContextOptionsBuilder<FilmRatingsDBContext>()
                .UseNpgsql("Server=localhost;Port=5432;Database=FilmRatingsDB;Uid=postgres;Pwd=postgres;");

            _context = new FilmRatingsDBContext(builder.Options);
            dataRepository = new UtilisateurManager(_context); 
            _controller = new UtilisateursController(dataRepository); 
        }

        [TestMethod]
        public void GetUtilisateurs_ReturnsAllUsers()
        {
            var result = _controller.GetUtilisateurs().Result;
            var actualUsers = (result.Value as IEnumerable<Utilisateur>)?.ToList();

            Assert.IsNotNull(actualUsers, "Результат не повинен бути null.");

            var expectedUsers = _context.Utilisateurs.ToList();

            foreach (var user in actualUsers)
            {
                bool existsInDb = expectedUsers.Any(u => u.UserId == user.UserId);
                Assert.IsTrue(existsInDb, $"Користувача з ID {user.UserId} не знайдено в базі даних.");
            }
        }

        [TestMethod]
        public void GetUtilisateurById_ReturnsUser_WhenUserExists()
        {
            var expectedUser = _context.Utilisateurs.FirstOrDefault(); 

            if (expectedUser == null) Assert.Inconclusive("База даних порожня, тест неможливо виконати.");

            var result = _controller.GetUtilisateurById(expectedUser.UserId).Result;

            Assert.IsNotNull(result.Value);
            Assert.AreEqual(expectedUser.UserId, result.Value.UserId);
        }

        [TestMethod]
        public void GetUtilisateurById_ReturnsNotFound_WhenUserDoesNotExist()
        {

            var result = _controller.GetUtilisateurById(99999).Result; 

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostUtilisateur_ModelValidated_CreationOK()
        {
            Random rnd = new Random();
            int digit = rnd.Next(1, 1000000000);

            Utilisateur userAtester = new Utilisateur()
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + digit + "@gmail.com", 
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                Cp = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France"
            };

            var result = _controller.PostUtilisateur(userAtester).Result;

            var userRecupere = _context.Utilisateurs.Where(u => u.Mail.ToLower() == userAtester.Mail.ToLower()).FirstOrDefault();
            Assert.IsNotNull(userRecupere);

            userAtester.UserId = userRecupere.UserId; 
            Assert.AreEqual(userRecupere.Mail, userAtester.Mail, "Users not identical");
        }

        [TestMethod]
        public void PostUtilisateur_ModelInvalid_ReturnsBadRequest()
        {
            Utilisateur user = new Utilisateur()
            {
                Mobile = "1", 
                Mail = "test@gmail.com",
                Pwd = "pwd"
            };

            string PhoneRegex = @"^0[0-9]{9}$";
            Regex regex = new Regex(PhoneRegex);

            if (!regex.IsMatch(user.Mobile))
            {
                _controller.ModelState.AddModelError("Mobile", "Le mobile doit contenir 10 chiffres");
            }

            var result = _controller.PostUtilisateur(user).Result;

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void DeleteUtilisateur_DeletesUser()
        {
            var userToDelete = new Utilisateur()
            {
                Nom = "Test",
                Mail = $"delete_{Guid.NewGuid()}@test.com",
                Pwd = "password"
            };
            _context.Utilisateurs.Add(userToDelete);
            _context.SaveChanges();
            int id = userToDelete.UserId;

            var result = _controller.DeleteUtilisateur(id).Result;

            var checkUser = _context.Utilisateurs.Find(id);
            Assert.IsNull(checkUser);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}