using VelaryMobile.Models;
using VelaryMobile.Pages.Auth;
using VelaryMobile.Services;

namespace VelaryMobile.Pages.Cliente
{
    public partial class CarrinhoPage : ContentPage
    {
        private readonly CarrinhoService _carrinhoService;
        private readonly AuthService _authService;
        private readonly ApiService _apiService;

        public CarrinhoPage(
            CarrinhoService carrinhoService,
            AuthService authService)
        {
            InitializeComponent();

            _carrinhoService = carrinhoService;
            _authService = authService;
            _apiService = new ApiService();

            AtualizarCarrinho();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AtualizarCarrinho();
        }

        private void AtualizarCarrinho()
        {
            CarrinhoCollectionView.ItemsSource = null;
            CarrinhoCollectionView.ItemsSource =
                _carrinhoService.Itens;

            int quantidadeTotal =
                _carrinhoService.Itens.Sum(item => item.Quantidade);

            QuantidadeItensLabel.Text =
                quantidadeTotal == 1
                    ? "1 item"
                    : $"{quantidadeTotal} itens";

            ResumoItensLabel.Text =
                quantidadeTotal == 1
                    ? "1 item"
                    : $"{quantidadeTotal} itens";

            decimal total =
                _carrinhoService.CalcularTotal();

            TotalLabel.Text =
                $"R$ {total:F2}";

            FinalizarButton.IsEnabled =
                _carrinhoService.Itens.Count > 0;
        }

        private async void OnFinalizarClicked(
            object sender,
            EventArgs e)
        {
            if (_carrinhoService.Itens.Count == 0)
            {
                await DisplayAlert(
                    "Carrinho vazio",
                    "Adicione pelo menos um produto antes de finalizar o pedido.",
                    "OK");

                return;
            }

            if (!_authService.EstaLogado ||
                _authService.UsuarioLogado == null)
            {
                await DisplayAlert(
                    "Login necessário",
                    "Faça login antes de finalizar o pedido.",
                    "OK");

                await Shell.Current.GoToAsync(
                    nameof(LoginPage));

                return;
            }

            bool confirmar = await DisplayAlert(
                "Finalizar pedido",
                $"Deseja finalizar seu pedido no valor de {TotalLabel.Text}?",
                "Sim",
                "Cancelar");

            if (!confirmar)
                return;

            try
            {
                FinalizarButton.IsEnabled = false;

                decimal total =
                    _carrinhoService.CalcularTotal();

                var pedido = new Pedido
                {
                    UsuarioId =
                        _authService.UsuarioLogado.Id,

                    ValorTotal = total,

                    Status = "Pendente",

                    Itens = _carrinhoService.Itens
                        .Select(item => new ItemPedido
                        {
                            ProdutoId = item.ProdutoId,
                            Quantidade = item.Quantidade,
                            PrecoUnitario =
                                item.PrecoUnitario
                        })
                        .ToList()
                };

                var resultado =
                    await _apiService.CriarPedidoAsync(pedido);

                if (resultado == null)
                {
                    await DisplayAlert(
                        "Erro ao finalizar",
                        "A API não confirmou a criação do pedido.",
                        "OK");

                    return;
                }

                _carrinhoService.Limpar();

                AtualizarCarrinho();

                await DisplayAlert(
                    "Pedido realizado",
                    $"Pedido #{resultado.Id} criado com sucesso!",
                    "OK");

                await Shell.Current.GoToAsync(
                    nameof(PedidosPage));
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro ao finalizar",
                    $"Não foi possível finalizar o pedido.\n\n{ex.Message}",
                    "OK");
            }
            finally
            {
                FinalizarButton.IsEnabled =
                    _carrinhoService.Itens.Count > 0;
            }
        }

        private async void OnProdutosClicked(
            object sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync(
                nameof(ProdutosPage));
        }
        private void OnRemoverClicked(object sender, EventArgs e)
        {
            if (sender is Button button &&
                button.CommandParameter is ItemPedido item)
            {
                _carrinhoService.Itens.Remove(item);
                AtualizarCarrinho();

            }
        }
    }
}