namespace BlazorApp1.Client.Services.ProductService
{
    public interface IProductService
    {
        event  Action ProductsChanged;
        List<Product> Products { get; set; }
        string Message { get; set; }
        Task GetProducts(string? categoryURL =null);
        Task<ServiceResponse<Product>> GetProduct(int productId);
        Task SearchProducts(string searchText);
        Task<List<string>> GetProductSearchSuggestion(string searchText);

    }
}
