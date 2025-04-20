using Microsoft.AspNetCore.Mvc;
using QuickStock.Domain.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace QuickStock.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7122/api/Category"; 

        public CategoryController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _httpClient.GetFromJsonAsync<List<Category>>(_apiUrl) ?? new();
            return View(categories);
        }
        // GET: /Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Category/Create
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            var response = await _httpClient.PostAsJsonAsync(_apiUrl, category);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "Error al registrar la categoría.");
            return View(category);
        }

        [HttpPost]
        // GET: /Category/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _httpClient.GetFromJsonAsync<Category>($"{_apiUrl}/{id}");
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: /Category/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{category.Id}", category);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "Error al actualizar la categoría.");
            return View(category);
        }

        // GET: /Category/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _httpClient.GetFromJsonAsync<Category>($"{_apiUrl}/{id}");
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: /Category/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "Error al eliminar la categoría.");
            return RedirectToAction("Delete", new { id });
        }


    }
}
