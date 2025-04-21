using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuickStock.Domain.Entities;
using QuickStock.Application.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace QuickStock.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7122/api/products";
        private readonly string _categoryApiUrl = "https://localhost:7122/api/categories";
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: /Product
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiUrl, _jsonOptions) ?? new();
                return View(products);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar productos: {ex.Message}";
                return View(new List<Product>());
            }
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

            try
            {
                // Crear el DTO para enviar a la API
                var dto = new ProductCreateDto
                {
                    Name = product.Name,
                    Size = product.Size,
                    Color = product.Color,
                    Price = product.Price,
                    Stock = product.Stock,
                    CategoryId = product.CategoryId
                };

                // Serializar manualmente para tener más control
                var json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud POST a la API
                var response = await _httpClient.PostAsync(_apiUrl, content);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Producto creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }

                // Si llegamos aquí, hubo un error
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Error al guardar el producto: {errorContent}");

                // Para depuración
                Console.WriteLine($"Error al crear producto: {errorContent}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                // Para depuración
                Console.WriteLine($"Excepción al crear producto: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }

            await LoadCategoriesAsync();
            return View(product);
        }

        // GET: /Product/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var product = JsonSerializer.Deserialize<Product>(content, _jsonOptions);

                    if (product == null)
                        return NotFound();

                    await LoadCategoriesAsync();
                    return View(product);
                }

                TempData["ErrorMessage"] = $"Error al cargar el producto: {response.StatusCode}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                TempData["ErrorMessage"] = "ID de producto no coincide";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(product);
            }

            try
            {
                // Crear un objeto anónimo con solo las propiedades necesarias
                var simplifiedProduct = new
                {
                    product.Id,
                    product.Name,
                    product.Size,
                    product.Color,
                    product.Stock,
                    product.Price,
                    product.CategoryId
                };

                // Serializar manualmente
                var json = JsonSerializer.Serialize(simplifiedProduct);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviar la solicitud PUT a la API
                var response = await _httpClient.PutAsync($"{_apiUrl}/{id}", content);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Producto actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }

                // Si llegamos aquí, hubo un error
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Error al actualizar el producto: {errorContent}");

                // Para depuración
                Console.WriteLine($"Error al actualizar producto: {errorContent}");
                Console.WriteLine($"JSON enviado: {json}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                // Para depuración
                Console.WriteLine($"Excepción al actualizar producto: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }

            await LoadCategoriesAsync();
            return View(product);
        }

        // GET: /Product/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _httpClient.GetFromJsonAsync<Product>($"{_apiUrl}/{id}", _jsonOptions);
                if (product == null)
                    return NotFound();

                return View(product);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Producto eliminado exitosamente";
                    return RedirectToAction(nameof(Index));
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Error al eliminar el producto: {errorContent}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Product/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var product = await response.Content.ReadFromJsonAsync<Product>();
                    if (product == null)
                        return NotFound();

                    return View(product);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound();

                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Error al cargar el producto: {errorContent}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Cargar categorías en el ViewBag
        private async Task LoadCategoriesAsync()
        {
            try
            {
                var categories = await _httpClient.GetFromJsonAsync<List<Category>>(_categoryApiUrl, _jsonOptions) ?? new();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
            }
            catch (Exception ex)
            {
                ViewBag.Categories = new SelectList(new List<Category>(), "Id", "Name");
                ModelState.AddModelError(string.Empty, $"No se pudieron cargar las categorías: {ex.Message}");
            }
        }
    }
}