using Microsoft.AspNetCore.Mvc;
using QuickStock.Domain.Entities;
using System.Net.Http;
using System.Net.Http.Json;

namespace QuickStock.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:port/api/Product"; 

        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_apiUrl) ?? new();
            return View(products);
        }
    }
}
