using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIfilms.Models.DataManager
{
    public class UtilisateurManager : IDataRepository<Utilisateur>
    {
        readonly FilmRatingsDBContext filmsDbContext = null!;

        public UtilisateurManager() { }

        public UtilisateurManager(FilmRatingsDBContext context)
        {
            filmsDbContext = context;
        }

        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetAllAsync()
        {
            return await filmsDbContext.Utilisateurs.ToListAsync();
        }

        public async Task<ActionResult<Utilisateur>> GetByIdAsync(int id)
        {
            var user = await filmsDbContext.Utilisateurs.FirstOrDefaultAsync(u => u.UserId == id);
            return user!;
        }

        public async Task<ActionResult<Utilisateur>> GetByStringAsync(string mail)
        {
            var user = await filmsDbContext.Utilisateurs.FirstOrDefaultAsync(u => u.Mail.ToUpper() == mail.ToUpper());
            return user!;
        }

        public async Task AddAsync(Utilisateur entity)
        {
            await filmsDbContext.Utilisateurs.AddAsync(entity);
            await filmsDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Utilisateur entityToUpdate, Utilisateur entity)
        {
            filmsDbContext.Entry(entityToUpdate).State = EntityState.Modified;

            entityToUpdate.UserId = entity.UserId;
            entityToUpdate.Nom = entity.Nom;
            entityToUpdate.Prenom = entity.Prenom;
            entityToUpdate.Mail = entity.Mail;
            entityToUpdate.Rue = entity.Rue;
            entityToUpdate.Cp = entity.Cp;
            entityToUpdate.Ville = entity.Ville;
            entityToUpdate.Pays = entity.Pays;
            entityToUpdate.Latitude = entity.Latitude;
            entityToUpdate.Longitude = entity.Longitude;
            entityToUpdate.Pwd = entity.Pwd;
            entityToUpdate.Mobile = entity.Mobile;

            await filmsDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Utilisateur entity)
        {
            filmsDbContext.Utilisateurs.Remove(entity);
            await filmsDbContext.SaveChangesAsync();
        }
    }
}