using Microsoft.AspNetCore.Mvc;
using QuickStock.Application.DTOS;
using QuickStock.Domain.Entities;
using QuickStock.Infrastructure.Repositories;

namespace QuickStock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryRepository _repository;

        public CategoriesController(CategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _repository.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            await _repository.AddAsync(category);
            return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (id != category.Id)
                return BadRequest("El ID proporcionado no coincide con el del objeto recibido.");

            await _repository.UpdateAsync(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
