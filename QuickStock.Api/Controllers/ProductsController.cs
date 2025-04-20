using Microsoft.AspNetCore.Mvc;
using QuickStock.Domain.Entities;
using QuickStock.Infrastructure.Repositories;
using QuickStock.Application.DTOs;
using QuickStock.Domain.Entities;



namespace QuickStock.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository _repository;

        public ProductsController(ProductRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _repository.GetAllAsync();
            return Ok(products);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)  return NotFound(); return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var product = new Product
                {
                    Name = dto.Name,
                    Size = dto.Size,
                    Color = dto.Color,
                    Stock = dto.Stock,
                    Price = dto.Price,
                    CategoryId = dto.CategoryId
                };

                await _repository.AddAsync(product);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error interno: {inner}");
            }

        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            Console.WriteLine($"🔍 ID en la URL: {id}, ID en el body: {product.Id}"); // 👈 Aquí

            if (id != product.Id)
                return BadRequest();

            await _repository.UpdateAsync(product);
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
