using System.Net.Http.Json;
using VelaryMobile.Models;
using VelaryMobile.Services;

namespace VelaryMobile.Pages.Admin
{
    public partial class AdminPedidosPage : ContentPage
    {
        private readonly ApiService _apiService;

        public AdminPedidosPage()
        {
            InitializeComponent();

            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CarregarPedidosAsync();
        }

        private async Task CarregarPedidosAsync()
        {
            try
            {
                var pedidos =
                    await _apiService.Client
                        .GetFromJsonAsync<List<Pedido>>(
                            "Pedidos");

                PedidosCollectionView.ItemsSource =
                    pedidos ?? new List<Pedido>();
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Não foi possível carregar os pedidos.\n\n{ex.Message}",
                    "OK");
            }
        }
    }
}