using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FakeStoreApp.Models
{
    public class ProductsRepoService : IProductRepo
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductsRepoService> _logger;
        private readonly IMemoryCache _cache;

        private static string GetAllProductsCacheKey => "all_products";
        private static string GetProductByIdCacheKey(int id) => $"products_id_{id}";
        private static string GetCategoriesCacheKey => "products_categories";
        private static string GetProductsByCategoryCacheKey(string category) => $"products_category_{category.ToLower()}";


        public ProductsRepoService(HttpClient httpClient, ILogger<ProductsRepoService> logger, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            if (_cache.TryGetValue(GetAllProductsCacheKey, out IEnumerable<ProductModel>? cached))
                return cached!;

            var response = await _httpClient.GetAsync("/products");
            if(!response.IsSuccessStatusCode)
                return Enumerable.Empty<ProductModel>();

            var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductModel>>() ?? Enumerable.Empty<ProductModel>();

            if (products.Any())
                _cache.Set(GetAllProductsCacheKey, products, TimeSpan.FromMinutes(10));

            return products;
        }

        public async Task<ProductModel?> GetProductByIdAsync(int Id)
        {
            string cacheKey = GetProductByIdCacheKey(Id);

            if (_cache.TryGetValue(cacheKey, out ProductModel? cached))
                return cached!;

            var response = await _httpClient.GetAsync($"/products/{Id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var product = await response.Content.ReadFromJsonAsync<ProductModel>();

            if (product != null)
                _cache.Set(cacheKey, product, TimeSpan.FromMinutes(10));

            return product! ;
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            if (_cache.TryGetValue(GetCategoriesCacheKey, out IEnumerable<string>? cached))
                return cached!;

            var response = await _httpClient.GetAsync("/products/categories");
            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<string>();

            var categories = await response.Content.ReadFromJsonAsync<IEnumerable<string>>() ?? Enumerable.Empty<string>();

            if (categories.Any())
                _cache.Set(GetCategoriesCacheKey, categories, TimeSpan.FromMinutes(5));

            return categories!;
        }

        public async Task<IEnumerable<ProductModel>> GetByCategoryAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return Enumerable.Empty<ProductModel>();

            string cacheKey = GetProductsByCategoryCacheKey(category);

            if (_cache.TryGetValue(cacheKey, out IEnumerable<ProductModel>? cached))
                return cached!;

            var response = await _httpClient.GetAsync($"/products/category/{category}");
            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<ProductModel>();

            var productsByCat = await response.Content.ReadFromJsonAsync<IEnumerable<ProductModel>>() ?? Enumerable.Empty<ProductModel>();

            if (productsByCat.Any())
                _cache.Set(cacheKey, productsByCat, TimeSpan.FromMinutes(10));

            return productsByCat!;
        }
    }
}
