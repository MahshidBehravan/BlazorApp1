using Blazored.LocalStorage;

namespace BlazorApp1.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        private async Task<bool> IsUserauthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public CartService(ILocalStorageService localStorage, HttpClient http,
                       AuthenticationStateProvider authStateProvider)
        {
            _localStorage = localStorage;
            _http = http;
            _authStateProvider = authStateProvider;
        }
        public event Action OnChanged;

       

        public async Task AddToCart(CartItem cartItem)
        {
            if((await IsUserauthenticated()))
            {
                Console.WriteLine("user is authenticated");
            }
            else
            {
                Console.WriteLine("User is not authenticated");
            }
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if(cart == null)
            {
                cart = new List<CartItem>();
            }

            var sameItem = cart.Find(x=>x.ProductId == cartItem.ProductId && x.ProductTypeId==cartItem.ProductTypeId);
            if(sameItem == null)
            {
                cart.Add(cartItem);
            }
            else
            {
                sameItem.Quantity+= cartItem.Quantity;
            }
          
            await _localStorage.SetItemAsync("cart", cart);
            await GetCartItemsCount();
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            await GetCartItemsCount();
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            return cart;
        }

        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            var CartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            var response = await _http.PostAsJsonAsync("api/cart/products", CartItems);
            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();
            return cartProducts.Data;
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }

            var cartItem=cart.Find
                (p=>p.ProductId == productId && p.ProductTypeId== productTypeId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                await _localStorage.SetItemAsync("cart",cart);
                await GetCartItemsCount();
            }
        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart == null)
            {
                return;
            }
            await _http.PostAsJsonAsync("api/cart", localCart);
            if (emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }
        }

        public async Task UpdateQuantity(CartProductResponse products)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }

            var cartItem = cart.Find
                (p => p.ProductId == products.ProductId && p.ProductTypeId == products.ProductTypeId);
            if (cartItem != null)
            {
                cartItem.Quantity=products.Quantity;
                await _localStorage.SetItemAsync("cart", cart);
            }
        }

        public async Task GetCartItemsCount()
        {
            if( await IsUserauthenticated())
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result.Data;

                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount",cart!=null ? cart.Count : 0);
            }
            OnChanged.Invoke();
            
        }
    }
}
