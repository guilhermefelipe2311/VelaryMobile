using System.Net.Http.Json;
using VelaryMobile.Models;
using VelaryMobile.Services;

namespace VelaryMobile.Pages.Admin
{
    public partial class AdminUsuariosPage : ContentPage
    {
        private readonly ApiService _apiService;

        private List<Usuario> _usuarios = new();

        public AdminUsuariosPage()
        {
            InitializeComponent();

            _apiService = new ApiService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CarregarUsuariosAsync();
        }

        // ==========================================
        // CARREGAR USUÁRIOS
        // ==========================================

        private async Task CarregarUsuariosAsync()
        {
            try
            {
                var usuarios =
                    await _apiService.Client
                        .GetFromJsonAsync<List<Usuario>>(
                            "Usuarios");

                _usuarios =
                    usuarios ?? new List<Usuario>();

                UsuariosCollectionView.ItemsSource =
                    _usuarios;
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Não foi possível carregar os usuários.\n\n{ex.Message}",
                    "OK");
            }
        }

        // ==========================================
        // ALTERAR TIPO
        // ==========================================

        private async void OnAlterarTipoClicked(
            object sender,
            EventArgs e)
        {
            if (sender is not Button button ||
                button.CommandParameter is not Usuario usuario)
            {
                return;
            }

            // PROTEGER ADMINISTRADOR PRINCIPAL

            if (usuario.Id == 1)
            {
                await DisplayAlert(
                    "Administrador principal",
                    "O administrador principal não pode ter o tipo alterado.",
                    "OK");

                return;
            }

            string novoTipo;

            if (string.Equals(
                    usuario.TipoUsuario,
                    "Admin",
                    StringComparison.OrdinalIgnoreCase))
            {
                novoTipo = "Cliente";
            }
            else
            {
                novoTipo = "Admin";
            }

            bool confirmar =
                await DisplayAlert(
                    "Alterar tipo",
                    $"Deseja alterar o usuário \"{usuario.Nome}\" para \"{novoTipo}\"?",
                    "Sim",
                    "Cancelar");

            if (!confirmar)
                return;

            try
            {
                var dados = new
                {
                    TipoUsuario = novoTipo
                };

                var response =
                    await _apiService.Client.PutAsJsonAsync(
                        $"Usuarios/{usuario.Id}",
                        dados);

                if (!response.IsSuccessStatusCode)
                {
                    string erro =
                        await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(erro))
                    {
                        erro =
                            $"A API retornou o status {(int)response.StatusCode} ({response.StatusCode}).";
                    }

                    await DisplayAlert(
                        "Erro",
                        $"Não foi possível alterar o tipo.\n\n{erro}",
                        "OK");

                    return;
                }

                await DisplayAlert(
                    "Sucesso",
                    $"O usuário agora é \"{novoTipo}\".",
                    "OK");

                await CarregarUsuariosAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Erro ao alterar o tipo do usuário.\n\n{ex.Message}",
                    "OK");
            }
        }

        // ==========================================
        // EXCLUIR USUÁRIO
        // ==========================================

        private async void OnExcluirClicked(
            object sender,
            EventArgs e)
        {
            if (sender is not Button button ||
                button.CommandParameter is not Usuario usuario)
            {
                return;
            }

            // PROTEGER ADMINISTRADOR PRINCIPAL

            if (usuario.Id == 1)
            {
                await DisplayAlert(
                    "Administrador principal",
                    "O administrador principal não pode ser excluído.",
                    "OK");

                return;
            }

            bool confirmar =
                await DisplayAlert(
                    "Excluir usuário",
                    $"Deseja realmente excluir o usuário \"{usuario.Nome}\"?",
                    "Excluir",
                    "Cancelar");

            if (!confirmar)
                return;

            try
            {
                var response =
                    await _apiService.Client.DeleteAsync(
                        $"Usuarios/{usuario.Id}");

                if (!response.IsSuccessStatusCode)
                {
                    string erro =
                        await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrWhiteSpace(erro))
                    {
                        erro =
                            $"A API retornou o status {(int)response.StatusCode} ({response.StatusCode}).";
                    }

                    await DisplayAlert(
                        "Erro",
                        $"Não foi possível excluir o usuário.\n\n{erro}",
                        "OK");

                    return;
                }

                await DisplayAlert(
                    "Sucesso",
                    "Usuário excluído com sucesso.",
                    "OK");

                await CarregarUsuariosAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Erro ao excluir usuário.\n\n{ex.Message}",
                    "OK");
            }
        }
    }
}