using Microsoft.AspNetCore.Mvc;
using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;

namespace APIfilms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly IDataRepository<Utilisateur> dataRepository;

        public UtilisateursController(IDataRepository<Utilisateur> dataRepo)
        {
            dataRepository = dataRepo;
        }

        [HttpGet("GetUtilisateurs")]
        [ActionName("GetUtilisateurs")]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            return await dataRepository.GetAllAsync();
        }

        [HttpGet("GetUtilisateurById/{id}")]
        [ActionName("GetUtilisateurById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurById(int id)
        {
            var user = await dataRepository.GetByIdAsync(id);
            if (user.Value == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpGet("GetUtilisateurByEmail/{email}")]
        [ActionName("GetUtilisateurByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurByEmail(string email)
        {
            var user = await dataRepository.GetByStringAsync(email);
            if (user.Value == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPut("PutUtilisateur/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.UserId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToUpdate = await dataRepository.GetByIdAsync(id);
            if (userToUpdate.Value == null)
            {
                return NotFound();
            }

            await dataRepository.UpdateAsync(userToUpdate.Value, utilisateur);
            return NoContent();
        }

        [HttpPost("PostUtilisateur")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(utilisateur);
            return CreatedAtAction("GetUtilisateurById", new { id = utilisateur.UserId }, utilisateur);
        }

        [HttpDelete("DeleteUtilisateur/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var user = await dataRepository.GetByIdAsync(id);
            if (user.Value == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(user.Value);
            return NoContent();
        }
    }
}