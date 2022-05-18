
namespace BlazorApp1.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }
        public List<Product> Products { get; set ; }=new List<Product>();
        public string Message { get; set; } = "Loading Products...";

        public event Action ProductsChanged;

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return result;
        }

        public async Task GetProducts(string? categoryURL = null)
        {
            var result = categoryURL == null ?
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/category/{categoryURL}");

            if (result!=null && result.Data!=null)
                Products = result.Data;

            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestion(string searchText)
        {
            var result = await _http.GetFromJsonAsync <ServiceResponse<List<String>>>($"api/product/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task SearchProducts(string searchText)
        {
            var result= await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{searchText}");
            if(result!=null && result.Data != null)
            {
                 Products= result.Data;
            }
            if (Products.Count == 0)
                Message = "No product found";
            ProductsChanged?.Invoke();

        }
    }
}
