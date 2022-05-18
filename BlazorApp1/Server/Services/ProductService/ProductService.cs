﻿
namespace BlazorApp1.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<List<Product>>> GetProductAsync()
        {
            var response= new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.ToListAsync()
            };
            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            var product= await _context.Products.FindAsync(productId);
            if (product == null && response.Data== null)
            {
                response.Success = false;
                response.Message = "Sorry, but this product does not available";
            }
            else
            {
                response.Data = product;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string URLcategoryURL)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.Where(p => p.Category.URL.ToLower().Equals( URLcategoryURL.ToLower())).ToListAsync()
        };
            return response;
        }
    }
}
