namespace BlazorApp1.Server.Services.Cartservice
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems);
    }
}
