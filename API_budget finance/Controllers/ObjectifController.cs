using API_budget_finance.Context;
using API_budget_finance.DTOs;
using API_budget_finance.Models;
using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        [HttpPost("suggestions")]
        public async Task<IActionResult> GenerateSuggestions(
            GenerateSuggestionsRequest request,
            [FromServices] IConfiguration configuration)
        {
            string? rawText = null;
            try
            {
                //var apiKey = configuration["GEMINI:APIKEY"];
                var client = new Client();

                var config = new GenerateContentConfig
                {
                    SystemInstruction = new Content
                    {
                        Parts = [new Part { Text = @"Tu es un assistant financier expert en planification de budget.
                        L'utilisateur va te decrire un projet (fete, vacances, evenement, etc.).
                        Utilise la recherche Google pour trouver des prix et couts realistes et actuels.
                        Tu dois repondre UNIQUEMENT avec un objet JSON au format suivant:
                        {""suggestions"":[{""description"":""description detaillee de la depense"",""title"":""titre court"",""price"":""prix en euros""}]}
                        - description: explication detaillee de la depense (2-3 phrases, pourquoi c'est necessaire, details pratiques)
                        - title: titre court et clair de la depense (maximum 50 caracteres)
                        - price: estimation du prix en euros (ex: ""200€"", ""50-80€"")
                        Pas de preambule, pas de texte avant ou apres le JSON, pas de code fences markdown.
                        Entre 5 et 10 suggestions." }]
                    },
                    Tools = [new Tool { GoogleSearch = new GoogleSearch() }]
                };

                var response = await client.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash",
                    contents: request.ProjectDescription,
                    config: config
                );

                rawText = response.Candidates?[0].Content?.Parts?[0].Text;
                if (string.IsNullOrEmpty(rawText))
                    return StatusCode(502, new { Error = "Gemini n'a pas retourne de reponse." });

                // Strip markdown code fences if present
                var json = rawText;
                if (json.Contains("```"))
                {
                    var start = json.IndexOf('{');
                    var end = json.LastIndexOf('}');
                    if (start >= 0 && end > start)
                        json = json.Substring(start, end - start + 1);
                }

                var parsed = JsonSerializer.Deserialize<GenerateSuggestionsResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var suggestions = parsed?.Suggestions ?? [];
                return Ok(new GenerateSuggestionsResponse { Suggestions = suggestions });
            }
            catch (ClientError ex)
            {
                return StatusCode(502, new { Error = $"Erreur Gemini: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"Erreur interne: {ex.Message}", RawResponse = rawText });
            }
        }
    }
    
}
