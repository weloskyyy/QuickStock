using Microsoft.AspNetCore.Mvc;
using QuickStock.Domain.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace QuickStock.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7122/api/products";

        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiUrl) ?? new();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var response = await _httpClient.PostAsJsonAsync(_apiUrl, product);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "Ocurrió un error al registrar el producto.");
            return View(product);
        }

        // GET: /Product/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<Product>($"{_apiUrl}/{id}");
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: /Product/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);

            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{product.Id}", product);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar el producto.");
            return View(product);
        }

        // GET: /Product/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<Product>($"{_apiUrl}/{id}");
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: /Product/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "No se pudo eliminar el producto.");
            return RedirectToAction("Delete", new { id });
        }


    }
}
