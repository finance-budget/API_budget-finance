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
    public class ObjectifController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthenticatedUserContext _authenticatedUserContext;

        public ObjectifController(AppDbContext context, IAuthenticatedUserContext authenticatedUserContext)
        {
            _context = context;
            _authenticatedUserContext = authenticatedUserContext;
        }
        [HttpGet]
        public IActionResult GetAllObjectifs()
        {
            var userId = _authenticatedUserContext.UserId;
            var objectifs = _context.Objectifs.Where(o => o.UserId == userId).Select(o => new
            {
                o.Id,
                o.Description,
                o.Atteint
            }).ToList();

            return Ok(objectifs);
        }



        [HttpGet("{id}")]
        public IActionResult GetObjectif(int id)
        {
            var userId = _authenticatedUserContext.UserId;
            var objectif = _context.Objectifs.FirstOrDefault(o => o.Id == id && o.UserId == userId);

            if (objectif == null)
            {
                return NotFound();
            }

            return Ok(objectif);
        }

        [HttpPost]
        public IActionResult CreateObjectif(AddObjectifsRequest objectifsRequest)
        {

            var userId = _authenticatedUserContext.UserId;
            Objectif objectif = new() {UserId= userId, Atteint=false, Description=objectifsRequest.Description };
            

            _context.Objectifs.Add(objectif);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetObjectif), new { id = objectif.Id }, new AddObjectifsResponse() { Id=objectif.Id, Atteint=objectif.Atteint, Description=objectif.Description});
        }


        [HttpPut("{id}")]
        public IActionResult UpdateObjectif(int id, Objectif updatedObjectif)
        {
            var existingObjectif = _context.Objectifs.FirstOrDefault(o => o.Id == id);
            if (existingObjectif == null)
            {
                return NotFound();
            }
            existingObjectif.Description = updatedObjectif.Description;
            existingObjectif.Atteint = updatedObjectif.Atteint;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteObjectif(int id)
        {
            var userId = _authenticatedUserContext.UserId;
            var objectif = _context.Objectifs.FirstOrDefault(o => o.Id == id && o.UserId == userId);

            if (objectif == null)
            {
                return NotFound();
            }

            _context.Objectifs.Remove(objectif);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id}/atteint")]
        public IActionResult UpdateObjectifAtteint(int id, UptadeObjectifAtteintRequest request)
        {
            var userId = _authenticatedUserContext.UserId;
            var objectif = _context.Objectifs.FirstOrDefault(o => o.Id == id && o.UserId == userId);

            if (objectif == null)
            {
                return NotFound();
            }

            objectif.Atteint = request.Atteint;
            _context.SaveChanges();

            return NoContent();
        }

    }
    
}
