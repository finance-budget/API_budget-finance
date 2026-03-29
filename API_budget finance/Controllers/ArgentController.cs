using API_budget_finance.Context;
using API_budget_finance.DTOs;
using API_budget_finance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_budget_finance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArgentController(AppDbContext context, IAuthenticatedUserContext authenticatedUserContext) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly IAuthenticatedUserContext _authenticatedUserContext = authenticatedUserContext;
        [HttpGet]
        public IActionResult GetArgents()
        {
            var userId = _authenticatedUserContext.UserId;
            // Logic to get the list of "Argent" entities for the authenticated user
            var argents = _context.Argents.Where(a => a.UserId == userId).Select(a => new 
            {
                a.Id,
                a.Type,
                a.Description,
                a.Montant,
                a.CategoryId,
                a.DateOperation
            }).ToList();
            return Ok(argents);
        }
        [HttpPost]
        public IActionResult CreateArgent(AddArgentRequest argent)
        {
            var userId = _authenticatedUserContext.UserId;
            var newArgent = new Argent
            {
                UserId = userId,
                Type = argent.Type,
                Description = argent.Description,
                Montant = argent.Montant,
                CategoryId = argent.CategoryId,
                DateOperation = DateTime.UtcNow

            };
            _context.Argents.Add(newArgent);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetArgents), new { id = newArgent.Id }, new AddArgentResponse { Id = newArgent.Id, Type = newArgent.Type, Description = newArgent.Description, Montant = newArgent.Montant, CategoryId = newArgent.CategoryId, DateOperation = newArgent.DateOperation });
        }
        [HttpPut("{id}")]
        public IActionResult UpdateArgent(int id, ModifArgent argent)
        {
            var userId = _authenticatedUserContext.UserId;
            var existingArgent = _context.Argents.FirstOrDefault(a => a.Id == id && a.UserId == userId);
            if (existingArgent == null)
            {
                return NotFound();
            }
            existingArgent.Type = argent.Type;
            existingArgent.Description = argent.Description;
            existingArgent.Montant = argent.Montant;
            existingArgent.CategoryId = argent.CategoryId;

            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteArgent(int id)
        {
            var userId = _authenticatedUserContext.UserId;
            var existingArgent = _context.Argents.FirstOrDefault(a => a.Id == id && a.UserId == userId);
            if (existingArgent == null)
            {
                return NotFound();
            }
            _context.Argents.Remove(existingArgent);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpGet("{id}")]
        public IActionResult GetArgentById(int id)
        {
            var userId = _authenticatedUserContext.UserId;
            var argent = _context.Argents.FirstOrDefault(a => a.Id == id && a.UserId == userId);
            if (argent == null)
            {
                return NotFound();
            }
            return Ok(argent);
        }
        [HttpGet("{id}/montant")]
        public IActionResult GetMontantArgentById(int id)
        {
            var userId = _authenticatedUserContext.UserId;
            var argent = _context.Argents.Select(a => new { a.Id, a.Montant ,a.UserId}).FirstOrDefault(a => a.Id == id && a.UserId == userId);
            if (argent == null)
            {
                return NotFound();
            }
            return Ok(argent.Montant);
        }
    }
}
