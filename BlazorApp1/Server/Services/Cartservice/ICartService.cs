namespace BlazorApp1.Server.Services.Cartservice
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems);
        Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems);
        Task<ServiceResponse<int>> GetCartItemCounts();
        Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts();

    }
}
