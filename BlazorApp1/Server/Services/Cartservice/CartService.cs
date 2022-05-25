namespace BlazorApp1.Server.Services.Cartservice
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;

        public CartService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponse>>
            {
                Data = new List<CartProductResponse>()
            };
            foreach (var item in cartItems)
            {
                var product = await _context.Products
                    .Where(p => p.Id == item.ProductId)
                    .FirstOrDefaultAsync();

                if(product == null)
                {
                    continue;
                }
                var ProductVariant = await _context.ProductVariants
                    .Where(v => v.ProductId == item.ProductId &&
                    v.ProductTypeId == item.ProductTypeId)
                    .Include(v => v.ProductType)
                    .FirstOrDefaultAsync();
                if(ProductVariant == null)
                {
                    continue;
                }
                var cartProduct = new CartProductResponse
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageURL = product.ImageURL,
                    Price = ProductVariant.Price,
                    ProductType = ProductVariant.ProductType.Name,
                    ProductTypeId = ProductVariant.ProductTypeId,
                    Quantity = item.Quantity
                };
                result.Data.Add(cartProduct);   
            }
            return result;
        }
    }
}
