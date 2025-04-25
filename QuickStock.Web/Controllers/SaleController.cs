using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickStock.Domain.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace QuickStock.Web.Controllers
{
    public class SaleController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7122/api/Sales";
        private readonly string _productApiUrl = "https://localhost:7122/api/products";

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
            var sale = await _httpClient.GetFromJsonAsync<Sale>($"{_apiUrl}/{id}");
            if (sale == null)
                return NotFound();

            await LoadProductsAsync(); // Cargar lista de productos
            return View(sale);
        }

        
        [HttpPost]
        public async Task<IActionResult> Edit(Sale sale)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{sale.Id}", sale);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "Error al actualizar la venta.");
            await LoadProductsAsync(); // Volver a cargar si falla
            return View(sale);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadProductsAsync();
            return View();
        }

        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var sale = await _httpClient.GetFromJsonAsync<Sale>($"{_apiUrl}/{id}");
            if (sale == null)
                return NotFound();

            return View(sale);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Sale sale)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiUrl, sale);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "Error al crear la venta.");
            await LoadProductsAsync();
            return View(sale);
        }

        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "No se pudo eliminar la venta.");
            return RedirectToAction("Delete", new { id });
        }




      
        private async Task LoadProductsAsync()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_productApiUrl);
            ViewBag.Products = new SelectList(products, "Id", "Name");
        }
    }
}
