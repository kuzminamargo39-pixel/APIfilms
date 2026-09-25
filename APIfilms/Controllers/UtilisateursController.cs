using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIfilms.Models.EntityFramework;

namespace APIfilms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly FilmRatingsDBContext _context;

        public UtilisateursController(FilmRatingsDBContext context)
        {
            _context = context;
        }

        // GET: api/Utilisateurs/GetUtilisateurs
        [HttpGet("GetUtilisateurs")]
        [ActionName("GetUtilisateurs")]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            return await _context.Utilisateurs.ToListAsync();
        }

        // GET: api/Utilisateurs/GetUtilisateurById/5
        [HttpGet("GetUtilisateurById/{id}")]
        [ActionName("GetUtilisateurById")]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurById(int id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return utilisateur;
        }

        // GET: api/Utilisateurs/GetUtilisateurByEmail/email@domaine.com
        [HttpGet("GetUtilisateurByEmail/{email}")]
        [ActionName("GetUtilisateurByEmail")]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurByEmail(string email)
        {
            var utilisateur = await _context.Utilisateurs
                                            .FirstOrDefaultAsync(u => u.Mail.ToLower() == email.ToLower());

            if (utilisateur == null)
            {
                return NotFound();
            }

            return utilisateur;
        }

        // PUT: api/Utilisateurs/PutUtilisateur/5
        [HttpPut("PutUtilisateur/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateurExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Utilisateurs/PostUtilisateur
        [HttpPost("PostUtilisateur")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilisateurById", new { id = utilisateur.UserId }, utilisateur);
        }

        // DELETE: api/Utilisateurs/DeleteUtilisateur/5
        [HttpDelete("DeleteUtilisateur/{id}")]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            _context.Utilisateurs.Remove(utilisateur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtilisateurExists(int id)
        {
            return _context.Utilisateurs.Any(e => e.UserId == id);
        }
    }
}