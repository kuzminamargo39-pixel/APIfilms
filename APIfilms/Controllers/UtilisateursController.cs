using Microsoft.AspNetCore.Mvc;
using APIfilms.Models.EntityFramework;
using APIfilms.Models.Repository;
using APIfilms.Models.DTO;
using AutoMapper;         

namespace APIfilms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly IDataRepository<Utilisateur> dataRepository;
        private readonly IMapper _mapper; 

        public UtilisateursController(IDataRepository<Utilisateur> dataRepo, IMapper mapper)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }

        [HttpGet("GetUtilisateurs")]
        [ActionName("GetUtilisateurs")]
        public async Task<ActionResult<IEnumerable<UtilisateurDto>>> GetUtilisateurs()
        {
            var result = await dataRepository.GetAllAsync();
            var utilisateurs = result.Value;

            if (utilisateurs == null)
            {
                return NotFound();
            }

            var dtos = _mapper.Map<IEnumerable<UtilisateurDto>>(utilisateurs);
            return Ok(dtos);
        }

        [HttpGet("GetUtilisateurById/{id}")]
        [ActionName("GetUtilisateurById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UtilisateurDetailDto>> GetUtilisateurById(int id)
        {
            var result = await dataRepository.GetByIdAsync(id);

            if (result == null || result.Value == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<UtilisateurDetailDto>(result.Value);
            return Ok(dto);
        }

        [HttpGet("GetUtilisateurByEmail/{email}")]
        [ActionName("GetUtilisateurByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UtilisateurDetailDto>> GetUtilisateurByEmail(string email)
        {
            var result = await dataRepository.GetByStringAsync(email);

            if (result == null || result.Value == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<UtilisateurDetailDto>(result.Value);
            return Ok(dto);
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

            if (userToUpdate == null || userToUpdate.Value == null)
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

            if (user == null || user.Value == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(user.Value);
            return NoContent();
        }
    }
}