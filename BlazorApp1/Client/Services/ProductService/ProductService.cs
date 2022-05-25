
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

        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public event Action ProductsChanged;

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return result;
        }

        public async Task GetProducts(string? categoryURL = null)
        {
            var result = categoryURL == null ?
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product/featured") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/category/{categoryURL}");

            if (result!=null && result.Data!=null)
                Products = result.Data;

            CurrentPage = 1;
            PageCount = 0;

            if (Products.Count == 0)
            {
                Message = "No Products Found";
            }
            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestion(string searchText)
        {
            var result = await _http.GetFromJsonAsync <ServiceResponse<List<String>>>($"api/product/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task SearchProducts(string searchText, int page)
        {
            LastSearchText = searchText;
            var result= await _http.GetFromJsonAsync<ServiceResponse<ProductSearchResult>>($"api/product/search/{searchText}/{page}");
            if(result!=null && result.Data != null)
            {
                 Products= result.Data.Products;
                CurrentPage=result.Data.CurrentPage;
                PageCount=result.Data.Pages;
            }
            if (Products.Count == 0)
                Message = "No product found";
            ProductsChanged?.Invoke();

        }
    }
}
