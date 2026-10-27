using System.Globalization;
using System.Net.Http.Json;
using VelaryMobile.Models;
using VelaryMobile.Services;

namespace VelaryMobile.Pages.Admin
{
    public partial class AdminProdutosPage : ContentPage
    {
        private readonly ApiService _apiService;

        private List<Produto> _produtos = new();

        public AdminProdutosPage()
        {
            InitializeComponent();

            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CarregarProdutosAsync();
        }

        private async Task CarregarProdutosAsync()
        {
            try
            {
                var produtos =
                    await _apiService.Client
                        .GetFromJsonAsync<List<Produto>>("Produtos");

                _produtos =
                    produtos ?? new List<Produto>();

                ProdutosCollectionView.ItemsSource =
                    _produtos;
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Não foi possível carregar os produtos.\n\n{ex.Message}",
                    "OK");
            }
        }

        private async void OnSalvarClicked(
            object sender,
            EventArgs e)
        {
            // ==========================================
            // VALIDAR NOME
            // ==========================================

            if (string.IsNullOrWhiteSpace(NomeEntry.Text))
            {
                await DisplayAlert(
                    "Produto",
                    "Informe o nome do produto.",
                    "OK");

                return;
            }

            // ==========================================
            // VALIDAR PREÇO
            // ==========================================

            string precoTexto =
                PrecoEntry.Text?.Trim() ?? string.Empty;

            if (!decimal.TryParse(
                    precoTexto,
                    NumberStyles.Number,
                    CultureInfo.GetCultureInfo("pt-BR"),
                    out decimal preco))
            {
                await DisplayAlert(
                    "Produto",
                    "Informe um preço válido.\n\nExemplo: 299,98",
                    "OK");

                return;
            }

            if (preco < 0)
            {
                await DisplayAlert(
                    "Produto",
                    "O preço não pode ser negativo.",
                    "OK");

                return;
            }

            // ==========================================
            // VALIDAR ESTOQUE
            // ==========================================

            string quantidadeTexto =
                QuantidadeEntry.Text?.Trim() ?? string.Empty;

            if (!int.TryParse(
                    quantidadeTexto,
                    out int quantidade))
            {
                await DisplayAlert(
                    "Produto",
                    "Informe uma quantidade válida.\n\nExemplo: 10",
                    "OK");

                return;
            }

            if (quantidade < 0)
            {
                await DisplayAlert(
                    "Produto",
                    "A quantidade não pode ser negativa.",
                    "OK");

                return;
            }

            // ==========================================
            // VALIDAR CATEGORIA
            // ==========================================

            string categoriaTexto =
                CategoriaIdEntry.Text?.Trim() ?? string.Empty;

            if (!int.TryParse(
                    categoriaTexto,
                    out int categoriaId))
            {
                await DisplayAlert(
                    "Produto",
                    "Informe um ID de categoria válido.\n\nExemplo: 1",
                    "OK");

                return;
            }

            if (categoriaId <= 0)
            {
                await DisplayAlert(
                    "Produto",
                    "Informe uma categoria válida.",
                    "OK");

                return;
            }

            // ==========================================
            // CRIAR PRODUTO
            // ==========================================

            try
            {
                var produto = new Produto
                {
                    // NÃO INFORMAMOS O ID.
                    // O banco deve gerar automaticamente.
                    Nome = NomeEntry.Text.Trim(),

                    Descricao =
                        DescricaoEntry.Text?.Trim() ?? string.Empty,

                    Preco = preco,

                    Quantidade = quantidade,

                    ImagemUrl =
                        ImagemUrlEntry.Text?.Trim() ?? string.Empty,

                    CategoriaId = categoriaId
                };

                // ==========================================
                // ENVIAR PARA A API
                // ==========================================

                var response =
                    await _apiService.Client.PostAsJsonAsync(
                        "Produtos",
                        produto);

                // ==========================================
                // VERIFICAR RESPOSTA DA API
                // ==========================================

                if (!response.IsSuccessStatusCode)
                {
                    string erroApi =
                        await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(erroApi))
                    {
                        erroApi =
                            $"A API retornou o status {(int)response.StatusCode} ({response.StatusCode}).";
                    }

                    await DisplayAlert(
                        "Erro ao cadastrar",
                        $"A API não conseguiu cadastrar o produto.\n\n" +
                        $"Status: {(int)response.StatusCode}\n\n" +
                        $"{erroApi}",
                        "OK");

                    return;
                }

                // ==========================================
                // SUCESSO
                // ==========================================

                var produtoCriado =
                    await response.Content.ReadFromJsonAsync<Produto>();

                string mensagemSucesso =
                    "Produto cadastrado com sucesso!";

                if (produtoCriado != null &&
                    produtoCriado.Id > 0)
                {
                    mensagemSucesso =
                        $"Produto cadastrado com sucesso!\n\n" +
                        $"ID gerado automaticamente: {produtoCriado.Id}";
                }

                await DisplayAlert(
                    "Sucesso",
                    mensagemSucesso,
                    "OK");

                LimparCampos();

                await CarregarProdutosAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Erro ao cadastrar produto.\n\n{ex.Message}",
                    "OK");
            }
        }

        private async void OnExcluirClicked(
            object sender,
            EventArgs e)
        {
            if (sender is not Button button ||
                button.CommandParameter is not Produto produto)
            {
                return;
            }

            bool confirmar =
                await DisplayAlert(
                    "Excluir produto",
                    $"Deseja excluir \"{produto.Nome}\"?",
                    "Sim",
                    "Cancelar");

            if (!confirmar)
                return;

            try
            {
                var response =
                    await _apiService.Client.DeleteAsync(
                        $"Produtos/{produto.Id}");

                if (!response.IsSuccessStatusCode)
                {
                    string erroApi =
                        await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(erroApi))
                    {
                        erroApi =
                            $"A API retornou o status {(int)response.StatusCode} ({response.StatusCode}).";
                    }

                    await DisplayAlert(
                        "Erro ao excluir",
                        erroApi,
                        "OK");

                    return;
                }

                await DisplayAlert(
                    "Sucesso",
                    "Produto excluído com sucesso.",
                    "OK");

                await CarregarProdutosAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Erro ao excluir produto.\n\n{ex.Message}",
                    "OK");
            }
        }

        private void OnLimparClicked(
            object sender,
            EventArgs e)
        {
            LimparCampos();
        }

        private void LimparCampos()
        {
            NomeEntry.Text = string.Empty;
            DescricaoEntry.Text = string.Empty;
            PrecoEntry.Text = string.Empty;
            QuantidadeEntry.Text = string.Empty;
            ImagemUrlEntry.Text = string.Empty;
            CategoriaIdEntry.Text = string.Empty;
        }
    }
}