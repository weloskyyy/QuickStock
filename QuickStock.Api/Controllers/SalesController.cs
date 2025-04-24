using Microsoft.AspNetCore.Mvc;
using QuickStock.Application.DTOS;
using QuickStock.Domain.Entities;
using QuickStock.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

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
            try
            {
                var sale = await _repository.GetByIdAsync(id);
                if (sale == null)
                    return NotFound();

                return Ok(sale);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR en GetById Sale: " + ex.Message);
                return StatusCode(500, "Error interno al obtener la venta.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SaleCreateDto saleDto)
        {
            try
            {
                // Validación básica
                if (saleDto.Quantity <= 0)
                    return BadRequest("La cantidad debe ser mayor que cero.");

                if (saleDto.UnitPrice <= 0)
                    return BadRequest("El precio unitario debe ser mayor que cero.");

                // Crear una nueva entidad Sale a partir del DTO
                var sale = new Sale
                {
                    ProductId = saleDto.ProductId,
                    Quantity = saleDto.Quantity,
                    UnitPrice = saleDto.UnitPrice,
                    Date = DateTime.Now,
                   
                };

                await _repository.AddAsync(sale);
                return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR en Create Sale: " + ex.Message);
                return StatusCode(500, "Error interno al crear la venta.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SaleCreateDto saleDto)
        {
            try
            {
                // Validación básica
                if (saleDto.Quantity <= 0)
                    return BadRequest("La cantidad debe ser mayor que cero.");

                if (saleDto.UnitPrice <= 0)
                    return BadRequest("El precio unitario debe ser mayor que cero.");

               
                var existingSale = await _repository.GetByIdAsync(id);
                if (existingSale == null)
                    return NotFound();

               
                existingSale.ProductId = saleDto.ProductId;
                existingSale.Quantity = saleDto.Quantity;
                existingSale.UnitPrice = saleDto.UnitPrice;
              

                await _repository.UpdateAsync(existingSale);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR en Update Sale: " + ex.Message);
                return StatusCode(500, "Error interno al actualizar la venta.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var sale = await _repository.GetByIdAsync(id);
                if (sale == null)
                    return NotFound();

                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR en Delete Sale: " + ex.Message);
                return StatusCode(500, "Error interno al eliminar la venta.");
            }
        }
    }
}