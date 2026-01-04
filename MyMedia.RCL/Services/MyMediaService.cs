using MyMedia.RCL.Models;
using System.Net.Http.Json;

namespace MyMedia.RCL.Services
{
    public class MyMediaService
    {
        private readonly HttpClient _httpClient;
        public event Action? OnChange;
        public string UserRole { get; private set; } = "";
        public bool IsSupplier => IsLoggedIn && (UserRole == "Fornecedor" || UserRole == "Admin");

        public string? JwtToken { get; private set; }
        public bool IsLoggedIn => !string.IsNullOrEmpty(JwtToken);
        private void NotifyStateChanged() => OnChange?.Invoke();

        public MyMediaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            if (!IsLoggedIn) return false;

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Products", product);

                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ ERRO AO CRIAR PRODUTO: {response.StatusCode} - {msg}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"✅ Produto criado com sucesso!");
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ ERRO CRÍTICO: {ex.Message}");
                return false;
            }
        }

       
        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                var products = await GetProductsAsync();
                return products
                    .Select(p => p.Category)
                    .Where(c => c != null)
                    .GroupBy(c => c.Id)
                    .Select(g => g.First())
                    .ToList();
            }
            catch { return new List<Category>(); }
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

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private string ParseRoleFromJwt(string token)
        {
            try
            {
                var payload = token.Split('.')[1];
                var jsonBytes = ParseBase64WithoutPadding(payload);
                var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

             
                object? roleObject = null;
                if (keyValuePairs!.TryGetValue("role", out object? r)) roleObject = r;
                else if (keyValuePairs.TryGetValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out object? r2)) roleObject = r2;

                if (roleObject != null)
                {
                    string roleStr = roleObject.ToString()!;

                    if (roleStr.Trim().StartsWith("["))
                    {
                        var roles = System.Text.Json.JsonSerializer.Deserialize<string[]>(roleStr);
                        if (roles != null)
                        {
                            if (roles.Contains("Admin")) return "Admin";
                            if (roles.Contains("Fornecedor")) return "Fornecedor";
                            return "Cliente";
                        }
                    }
                    else
                    {

                        if (roleStr == "Admin") return "Admin";
                        if (roleStr == "Fornecedor") return "Fornecedor";
                    }
                }
                return "Cliente";
            }
            catch
            {
                return "Cliente";
            }
        }

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
                            UserRole = ParseRoleFromJwt(JwtToken);
                    }
                    NotifyStateChanged();
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
            NotifyStateChanged();
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

        public async Task<Product?> GetHighlightProductAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Product>("api/Products/highlight");
            }
            catch (Exception)
            {
                return null; 
            }
        }

        public async Task<List<Product>> GetFavoritesAsync()
        {
            if (!IsLoggedIn) return new List<Product>();

            try
            {
                // Tenta fazer o pedido
                var response = await _httpClient.GetAsync("api/Favorites");

                // Se a API der erro, mostra qual foi!
                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ ERRO API FAVORITOS: {response.StatusCode} - {msg}");
                    System.Diagnostics.Debug.WriteLine($"❌ ERRO API FAVORITOS: {response.StatusCode} - {msg}");
                    return new List<Product>();
                }

                return await response.Content.ReadFromJsonAsync<List<Product>>() ?? new List<Product>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERRO CRÍTICO: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ ERRO CRÍTICO: {ex.Message}");
                return new List<Product>();
            }
        }

        public async Task ToggleFavoriteAsync(int productId, bool isCurrentlyFavorite)
        {
            if (!IsLoggedIn) return;

            HttpResponseMessage response;
            try
            {
                if (isCurrentlyFavorite)
                    response = await _httpClient.DeleteAsync($"api/Favorites/{productId}");
                else
                    response = await _httpClient.PostAsync($"api/Favorites/{productId}", null);

                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"❌ ERRO FAVORITOS: {response.StatusCode} - {msg}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"✅ SUCESSO FAVORITOS!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ ERRO CRÍTICO: {ex.Message}");
            }
        }

        public async Task<bool> IsProductFavoriteAsync(int productId)
        {
            if (!IsLoggedIn) return false;
            try
            {
                return await _httpClient.GetFromJsonAsync<bool>($"api/Favorites/check/{productId}");
            }
            catch { return false; }
        }
    }
}
