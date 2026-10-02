using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIfilms.Models.DataManager
{
    public class FilmManager : IDataRepository<Film>
    {
        private readonly FilmRatingsDBContext filmsDbContext;

        public FilmManager(FilmRatingsDBContext context)
        {
            filmsDbContext = context;
        }

        public async Task<ActionResult<IEnumerable<Film>>> GetAllAsync()
        {
            return await filmsDbContext.Films.ToListAsync();
        }

        public async Task<ActionResult<Film>> GetByIdAsync(int id)
        {
            return await filmsDbContext.Films.FirstOrDefaultAsync(f => f.FilmId == id);
        }

        public async Task<ActionResult<Film>> GetByStringAsync(string titre)
        {
            return await filmsDbContext.Films.FirstOrDefaultAsync(f => f.Titre.ToUpper() == titre.ToUpper());
        }

        public async Task AddAsync(Film entity)
        {
            await filmsDbContext.Films.AddAsync(entity);
            await filmsDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Film entityToUpdate, Film entity)
        {
            filmsDbContext.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await filmsDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Film entity)
        {
            filmsDbContext.Films.Remove(entity);
            await filmsDbContext.SaveChangesAsync();
        }
    }
}
