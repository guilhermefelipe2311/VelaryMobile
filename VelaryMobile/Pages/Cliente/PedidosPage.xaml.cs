using VelaryMobile.Models;
using VelaryMobile.Services;

namespace VelaryMobile.Pages.Cliente
{
    public partial class PedidosPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly AuthService _authService;

        private readonly List<Pedido> _pedidos = new();

        public PedidosPage(
            AuthService authService)
        {
            InitializeComponent();

            _apiService = new ApiService();
            _authService = authService;

            PedidosCollectionView.ItemsSource =
                _pedidos;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CarregarPedidosAsync();
        }

        private async Task CarregarPedidosAsync()
        {
            if (!_authService.EstaLogado ||
                _authService.UsuarioLogado == null)
            {
                await DisplayAlert(
                    "Login necessário",
                    "Faça login para visualizar seus pedidos.",
                    "OK");

                await Shell.Current.GoToAsync(
                    "LoginPage");

                return;
            }

            try
            {
                var pedidos =
                    await _apiService.GetPedidosAsync();

                var usuarioId =
                    _authService.UsuarioLogado.Id;

                var pedidosDoUsuario =
                    pedidos
                        .Where(p =>
                            p.UsuarioId == usuarioId)
                        .OrderByDescending(
                            p => p.DataPedido)
                        .ToList();

                _pedidos.Clear();

                _pedidos.AddRange(
                    pedidosDoUsuario);

                PedidosCollectionView.ItemsSource = null;

                PedidosCollectionView.ItemsSource =
                    _pedidos;
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Não foi possível carregar seus pedidos.\n\n{ex.Message}",
                    "OK");
            }
        }
    }
}