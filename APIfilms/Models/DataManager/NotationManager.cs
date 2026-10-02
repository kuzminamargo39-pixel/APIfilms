using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIfilms.Models.DataManager
{
    public class NotationManager : IDataRepository<Notation>
    {
        private readonly FilmRatingsDBContext filmsDbContext;

        public NotationManager(FilmRatingsDBContext context)
        {
            filmsDbContext = context;
        }

        public async Task<ActionResult<IEnumerable<Notation>>> GetAllAsync()
        {
            return await filmsDbContext.Notations.ToListAsync();
        }

        public async Task<ActionResult<Notation>> GetByIdAsync(int id)
        {
            return await filmsDbContext.Notations.FirstOrDefaultAsync(n => n.UtilisateurId == id);
        }

        public async Task<ActionResult<Notation>> GetByStringAsync(string str)
        {
            return null;
        }

        public async Task AddAsync(Notation entity)
        {
            await filmsDbContext.Notations.AddAsync(entity);
            await filmsDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Notation entityToUpdate, Notation entity)
        {
            filmsDbContext.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await filmsDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Notation entity)
        {
            filmsDbContext.Notations.Remove(entity);
            await filmsDbContext.SaveChangesAsync();
        }
    }
}