using MyMedia.RCL.Models;
using System.Net.Http.Json;

namespace MyMedia.RCL.Services
{
    public class MyMediaService
    {
        private readonly HttpClient _httpClient; 

        //Construtor
        public MyMediaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var products = await _httpClient.GetFromJsonAsync<List<Product>>("api/Products");
                return products ?? new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro a encontrar produtos: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"ERRO API: {ex.Message}");
                return new List<Product>();
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Product>($"api/Products/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar produto {id}: {ex.Message}");
                return null;
            }
        }

        public string? JwtToken { get; private set; }

        public bool IsLoggedIn => !string.IsNullOrEmpty(JwtToken);

        public async Task<string?> LoginAsync(LoginModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/login", model);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    JwtToken = result?.Token;

                    if (JwtToken != null)
                    {
                        _httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JwtToken);
                    }

                    return null; 
                }
                else
                {
                    return "Email ou Password inválidos.";
                }
            }
            catch (Exception ex)
            {
                return $"Erro de ligação: {ex.Message}";
            }
        }

        public async Task<string?> RegisterAsync(RegisterModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Auth/register", model);

                if (response.IsSuccessStatusCode)
                {
                    return null; 
                }
                else
                {                    var errorMsg = await response.Content.ReadAsStringAsync();
                    return $"Erro no registo: {errorMsg}";
                }
            }
            catch (Exception ex)
            {
                return $"Erro de ligação: {ex.Message}";
            }
        }

        public async Task LogoutAsync()
        {
            JwtToken = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> CheckoutAsync(List<CartItem> cartItems)
        {
            try
            {
                var orderDto = new
                {
                    Items = cartItems.Select(i => new { ProductId = i.Product.Id, Quantity = i.Quantity }).ToList()
                };

                var response = await _httpClient.PostAsJsonAsync("api/Orders", orderDto);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no checkout: {ex.Message}");
                return false;
            }
        }

        public async Task<List<OrderModel>> GetMyOrdersAsync()
        {
            try
            {
                var orders = await _httpClient.GetFromJsonAsync<List<OrderModel>>("api/Orders/my-orders");
                return orders ?? new List<OrderModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter encomendas: {ex.Message}");
                return new List<OrderModel>();
            }
        }
    }
}
