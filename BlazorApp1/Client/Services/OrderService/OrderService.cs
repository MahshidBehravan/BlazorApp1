using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        public OrderService(HttpClient http,
            AuthenticationStateProvider authenticationStateProvider,
            NavigationManager navigationManager
            )
        {
            _http = http;
            _authStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }
        public async Task PlaceOrder()
        {
            if(await IsUserauthenticated())
            {
                await _http.PostAsync("api/order",null);
            }
            else
            {
                _navigationManager.NavigateTo("login");
            }
        }

        private async Task<bool> IsUserauthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
