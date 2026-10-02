using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using APIfilms.Profiles;

namespace APIfilmsTests
{
    [TestClass]
    public class AutoMapperTests
    {
        [TestMethod]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<UtilisateurProfile>());

            config.AssertConfigurationIsValid();
        }
    }
}
