using Microsoft.AspNetCore.Mvc;
using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;

namespace APIfilms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmsController : ControllerBase
    {
        private readonly IDataRepository<Film> dataRepository;

        public FilmsController(IDataRepository<Film> dataRepo)
        {
            dataRepository = dataRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
        {
            return await dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
        {
            var result = await dataRepository.GetByIdAsync(id);

            if (result == null || result.Value == null)
            {
                return NotFound();
            }

            return result.Value;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(int id, Film film)
        {
            if (id != film.FilmId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filmToUpdate = await dataRepository.GetByIdAsync(id);
            if (filmToUpdate == null || filmToUpdate.Value == null)
            {
                return NotFound();
            }

            await dataRepository.UpdateAsync(filmToUpdate.Value, film);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Film>> PostFilm(Film film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(film);
            return CreatedAtAction("GetFilm", new { id = film.FilmId }, film);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            var film = await dataRepository.GetByIdAsync(id);
            if (film == null || film.Value == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(film.Value);
            return NoContent();
        }
    }
}