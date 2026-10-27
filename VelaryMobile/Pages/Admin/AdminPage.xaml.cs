using System.Net.Http.Json;
using VelaryMobile.Models;
using VelaryMobile.Pages.Cliente;
using VelaryMobile.Services;

namespace VelaryMobile.Pages.Admin
{
    public partial class AdminPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly AuthService _authService;

        public AdminPage(AuthService authService)
        {
            InitializeComponent();

            _apiService = new ApiService();
            _authService = authService;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CarregarDashboardAsync();
        }

        private async Task CarregarDashboardAsync()
        {
            try
            {
                var produtos =
                    await _apiService.Client
                        .GetFromJsonAsync<List<Produto>>("Produtos");

                var pedidos =
                    await _apiService.Client
                        .GetFromJsonAsync<List<Pedido>>("Pedidos");

                var usuarios =
                    await _apiService.Client
                        .GetFromJsonAsync<List<Usuario>>("Usuarios");

                ProdutosTotalLabel.Text =
                    (produtos?.Count ?? 0).ToString();

                PedidosTotalLabel.Text =
                    (pedidos?.Count ?? 0).ToString();

                UsuariosTotalLabel.Text =
                    (usuarios?.Count ?? 0).ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Não foi possível carregar o dashboard.\n\n{ex.Message}",
                    "OK");
            }
        }

        private async void OnProdutosClicked(
            object sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync(
                "AdminProdutosPage");
        }

        private async void OnPedidosClicked(
            object sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync(
                "AdminPedidosPage");
        }

        private async void OnUsuariosClicked(
            object sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync(
                "AdminUsuariosPage");
        }

        private async void OnSairClicked(
            object sender,
            EventArgs e)
        {
            bool confirmar =
                await DisplayAlert(
                    "Sair",
                    "Deseja realmente sair da área administrativa?",
                    "Sim",
                    "Cancelar");

            if (!confirmar)
                return;

            _authService.Logout();

            await Shell.Current.GoToAsync(
                "//MainPage");
        }
    }
}