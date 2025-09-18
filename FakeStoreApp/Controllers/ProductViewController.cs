using FakeStoreApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Numerics;

namespace FakeStoreApp.Controllers
{
    public class ProductViewController : Controller
    {

        private readonly IProductRepo _repo;
        private readonly ILogger<ProductViewController> _logger;


        public ProductViewController(IProductRepo repo, ILogger<ProductViewController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // GET: ProductViewController
        public async Task<IActionResult> Index(string? category = null, string? search = null, string? sort = null, int page = 1, int pageSize = 6)
        {

            var products = await _repo.GetAllProductsAsync();

            if (!string.IsNullOrEmpty(category)) {
                products = products.Where(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(x => x.Title.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            products = sort switch
            {
                "price_asc" => products.OrderBy(x => x.Price),
                "price_desc" => products.OrderByDescending(x => x.Price),
                "name_asc" => products.OrderBy(x => x.Title),
                "name_desc" => products.OrderByDescending(x => x.Title),
                _ => products
            };

            var productCount = products.Count();
            var pagedProduct = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductGridPartial", (pagedProduct, productCount, page, pageSize));
            }

            return View((pagedProduct, productCount, page, pageSize));
        }

        // GET: ProductViewController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);

            return View(product);
        }

        public async Task<IActionResult> ByCategory(string category)
        {
            var products = await _repo.GetByCategoryAsync(category);

            return View("Index", products);
        }

        // GET: ProductViewController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductViewController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}
