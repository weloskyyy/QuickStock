using Microsoft.AspNetCore.Mvc;
using QuickStock.Domain.Entities;
using QuickStock.Infrastructure.Repositories;
using QuickStock.Application.DTOs;




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
                {
                    return BadRequest(ModelState);
                }

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
                // Registrar el error para depuración
                System.Diagnostics.Debug.WriteLine($"Error en API al crear producto: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
        {
            try
            {
                if (id != dto.Id)
                    return BadRequest("El ID en la URL no coincide con el ID en el cuerpo de la solicitud");

                var existingProduct = await _repository.GetByIdAsync(id);
                if (existingProduct == null)
                    return NotFound($"No se encontró un producto con ID {id}");

                // Actualizar solo las propiedades necesarias
                existingProduct.Name = dto.Name;
                existingProduct.Size = dto.Size;
                existingProduct.Color = dto.Color;
                existingProduct.Stock = dto.Stock;
                existingProduct.Price = dto.Price;
                existingProduct.CategoryId = dto.CategoryId;

                await _repository.UpdateAsync(existingProduct);
                return NoContent();
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error interno: {inner}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }




    }

}
