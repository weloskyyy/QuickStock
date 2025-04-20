using Microsoft.AspNetCore.Mvc;
using QuickStock.Domain.Entities;
using QuickStock.Infrastructure.Repositories;

namespace QuickStock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly SaleRepository _repository;

        public SalesController(SaleRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var sales = await _repository.GetAllAsync();
                return Ok(sales);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR en GetAll Sales: " + ex.Message);
                return StatusCode(500, "Error interno en la API de ventas.");
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Sale sale)
        {
            await _repository.AddAsync(sale);
            return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Sale sale)
        {
            if (id != sale.Id)
                return BadRequest("El ID no coincide.");

            await _repository.UpdateAsync(sale);
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

