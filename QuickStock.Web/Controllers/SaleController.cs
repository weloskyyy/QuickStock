using Microsoft.AspNetCore.Mvc;
using QuickStock.Domain.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace QuickStock.Web.Controllers
{
    public class SaleController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7122/api/Sale";

        public SaleController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var sales = await _httpClient.GetFromJsonAsync<List<Sale>>(_apiUrl) ?? new();
            return View(sales);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var sale = await _httpClient.GetFromJsonAsync<Sale>($"https://localhost:7122/api/Sales/{id}");
            if (sale == null)
                return NotFound();

            return View(sale);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Sale sale)
        {
            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7122/api/Sales/{sale.Id}", sale);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "Error al actualizar la venta.");
            return View(sale);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var sale = await _httpClient.GetFromJsonAsync<Sale>($"https://localhost:7122/api/Sales/{id}");
            if (sale == null)
                return NotFound();

            return View(sale);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7122/api/Sales/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "No se pudo eliminar la venta.");
            return RedirectToAction("Delete", new { id });
        }

    }
}
