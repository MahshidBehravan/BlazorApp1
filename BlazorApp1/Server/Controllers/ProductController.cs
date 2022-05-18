using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var products = await _productService.GetProductAsync();
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProduct(int productId)
        {
            var products = await _productService.GetProductAsync(productId);
            return Ok(products);
        }
        [HttpGet("category/{categoryURL}")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetProductByCategory(string categoryURL)
        {
            var result = await _productService.GetProductsByCategory(categoryURL);
            return Ok(result);

        }
        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> SearhcProducts(string searchText)
        {
            var result = await _productService.SearchProducts(searchText);
            return Ok(result);

        }

        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetProductSearchSuggestions(string searchText)
        {
            var result = await _productService.GetProductSearchSuggestion(searchText);
            return Ok(result);

        }

    }
}
