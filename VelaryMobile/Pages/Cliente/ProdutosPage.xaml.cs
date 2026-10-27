using VelaryMobile.Models;
using VelaryMobile.Services;

namespace VelaryMobile.Pages.Cliente
{
    public partial class ProdutosPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly CarrinhoService _carrinhoService;
        private readonly AuthService _authService;

        private readonly List<Produto> _produtos = new();

        private List<Produto> _produtosFiltrados = new();

        public ProdutosPage(
            CarrinhoService carrinhoService,
            AuthService authService)
        {
            InitializeComponent();

            _apiService = new ApiService();
            _carrinhoService = carrinhoService;
            _authService = authService;

            ProdutosCollectionView.ItemsSource =
                _produtosFiltrados;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            CarregarUsuario();

            await CarregarProdutosAsync();
        }

        private void CarregarUsuario()
        {
            var usuario = _authService.UsuarioLogado;

            if (usuario != null)
            {
                SaudacaoLabel.Text =
                    $"Olá, {usuario.Nome}!";
            }
            else
            {
                SaudacaoLabel.Text = "Olá!";
            }
        }

        private async Task CarregarProdutosAsync()
        {
            try
            {
                var produtos =
                    await _apiService.GetProdutosAsync();

                _produtos.Clear();
                _produtos.AddRange(produtos);

                _produtosFiltrados =
                    new List<Produto>(_produtos);

                ProdutosCollectionView.ItemsSource =
                    _produtosFiltrados;
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Não foi possível carregar os produtos.\n\n{ex.Message}",
                    "OK");
            }
        }

        private void OnBuscaTextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            string busca =
                e.NewTextValue?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(busca))
            {
                _produtosFiltrados =
                    new List<Produto>(_produtos);
            }
            else
            {
                _produtosFiltrados =
                    _produtos
                        .Where(p =>
                            p.Nome.Contains(
                                busca,
                                StringComparison.OrdinalIgnoreCase) ||
                            p.Descricao.Contains(
                                busca,
                                StringComparison.OrdinalIgnoreCase))
                        .ToList();
            }

            ProdutosCollectionView.ItemsSource =
                _produtosFiltrados;
        }

        private async void OnProdutoSelecionado(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault()
                is not Produto produto)
            {
                return;
            }

            ((CollectionView)sender).SelectedItem = null;

            _carrinhoService.AdicionarProduto(produto);

            await DisplayAlert(
                "Carrinho",
                $"{produto.Nome} foi adicionado ao carrinho.",
                "OK");
        }

        private async void OnCarrinhoClicked(
            object sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync(
                nameof(CarrinhoPage));
        }

        private async void OnPerfilClicked(
            object sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync(
                nameof(PerfilPage));
        }
    }
}