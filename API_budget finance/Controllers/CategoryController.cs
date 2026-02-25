using API_budget_finance.DTOs;
using API_budget_finance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_budget_finance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        [HttpGet]
        public IActionResult GetCategories()
        {
            // Logic to get the list of "Category" entities for the authenticated user
            return new OkObjectResult(_context.Categories.ToList());
        }
        [HttpPost]
        public IActionResult CreateCategory(AddCategoryRequest category)
        {
            // Logic to create a new "Category" entity for the authenticated user
            var newCategory = new Category
            {
                Name = category.Name
            };
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return new OkObjectResult(new AddCategoryResponse { Id = newCategory.Id, Name = newCategory.Name });
        }
    }
}
