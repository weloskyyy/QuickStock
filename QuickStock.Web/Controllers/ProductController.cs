using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickStock.Domain.Entities;
using QuickStock.Application.DTOs;
using System.Net.Http;
using System.Net.Http.Json;

namespace QuickStock.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7122/api/products";
        private readonly string _categoryApiUrl = "https://localhost:7122/api/categories";

        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiUrl) ?? new();
            return View(products);
        }

        // GET: /Product/Create
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync();
            return View();
        }

        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(product);
            }

            var dto = new ProductCreateDto
            {
                Name = product.Name,
                Size = product.Size,
                Color = product.Color,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId
            };

            var response = await _httpClient.PostAsJsonAsync(_apiUrl, dto);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "Error al guardar el producto.");
            await LoadCategoriesAsync();
            return View(product);
        }

        // GET: /Product/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _httpClient.GetFromJsonAsync<Product>($"{_apiUrl}/{id}");
            if (product == null)
                return NotFound();

            await LoadCategoriesAsync();
            return View(product);
        }



        // POST: /Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(product);
            }

            // Make sure we're sending the ID in both the URL and the request body
            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{product.Id}", product);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            // If we get here, something went wrong
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Error al actualizar el producto: {errorContent}");
            await LoadCategoriesAsync();
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

        // Cargar categorías en el ViewBag
        private async Task LoadCategoriesAsync()
        {
            var categories = await _httpClient.GetFromJsonAsync<List<Category>>(_categoryApiUrl);
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }
    }
}
