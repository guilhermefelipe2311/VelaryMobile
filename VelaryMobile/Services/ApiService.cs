using System.Net.Http.Json;
using VelaryMobile.Models;

namespace VelaryMobile.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://10.31.244.49:7071/api/")
            };
        }

        public HttpClient Client => _httpClient;


        // =========================
        // PRODUTOS
        // =========================

        public async Task<List<Produto>> GetProdutosAsync()
        {
            var produtos =
                await _httpClient.GetFromJsonAsync<List<Produto>>("Produtos");

            return produtos ?? new List<Produto>();
        }


        // =========================
        // USUÁRIOS - CADASTRO
        // =========================

        public async Task<Usuario?> CadastrarUsuarioAsync(Usuario usuario)
        {
            var response =
                await _httpClient.PostAsJsonAsync("Usuarios", usuario);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<Usuario>();
        }


        // =========================
        // USUÁRIOS - LOGIN
        // =========================

        public async Task<Usuario?> LoginAsync(string email, string senha)
        {
            var dados = new
            {
                Email = email,
                Senha = senha
            };

            var response =
                await _httpClient.PostAsJsonAsync("Usuarios/login", dados);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<Usuario>();
        }


        // =========================
        // PEDIDOS - LISTAR
        // =========================

        public async Task<List<Pedido>> GetPedidosAsync()
        {
            var pedidos =
                await _httpClient.GetFromJsonAsync<List<Pedido>>("Pedidos");

            return pedidos ?? new List<Pedido>();
        }


        // =========================
        // PEDIDOS - CRIAR
        // =========================

        public async Task<Pedido?> CriarPedidoAsync(Pedido pedido)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "Pedidos",
                pedido);

            if (!response.IsSuccessStatusCode)
            {
                var erro = await response.Content.ReadAsStringAsync();

                throw new Exception(
                    $"API retornou {(int)response.StatusCode} ({response.StatusCode}).\n\n{erro}");
            }

            return await response.Content.ReadFromJsonAsync<Pedido>();
        }
    }
}