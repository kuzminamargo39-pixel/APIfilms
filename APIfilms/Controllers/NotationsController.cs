using Microsoft.AspNetCore.Mvc;
using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;

namespace APIfilms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotationsController : ControllerBase
    {
        private readonly IDataRepository<Notation> dataRepository;

        public NotationsController(IDataRepository<Notation> dataRepo)
        {
            dataRepository = dataRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notation>>> GetNotations()
        {
            return await dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notation>> GetNotation(int id)
        {
            var result = await dataRepository.GetByIdAsync(id);

            if (result == null || result.Value == null)
            {
                return NotFound();
            }

            return result.Value;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotation(int id, Notation notation)
        {
            if (id != notation.UtilisateurId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notationToUpdate = await dataRepository.GetByIdAsync(id);
            if (notationToUpdate == null || notationToUpdate.Value == null)
            {
                return NotFound();
            }

            await dataRepository.UpdateAsync(notationToUpdate.Value, notation);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Notation>> PostNotation(Notation notation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(notation);
            return CreatedAtAction("GetNotation", new { id = notation.UtilisateurId }, notation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotation(int id)
        {
            var notation = await dataRepository.GetByIdAsync(id);
            if (notation == null || notation.Value == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(notation.Value);
            return NoContent();
        }
    }
}