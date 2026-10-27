using VelaryMobile.Pages.Admin;
using VelaryMobile.Pages.Cliente;
using VelaryMobile.Services;

namespace VelaryMobile.Pages.Auth
{
    public partial class LoginPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly AuthService _authService;

        public LoginPage(AuthService authService)
        {
            InitializeComponent();

            _apiService = new ApiService();
            _authService = authService;
        }

        private async void OnEntrarClicked(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) ||
                string.IsNullOrWhiteSpace(SenhaEntry.Text))
            {
                await DisplayAlert(
                    "Login",
                    "Informe o e-mail e a senha.",
                    "OK");

                return;
            }

            try
            {
                EntrarButton.IsEnabled = false;

                var usuario =
                    await _apiService.LoginAsync(
                        EmailEntry.Text.Trim(),
                        SenhaEntry.Text);

                if (usuario == null)
                {
                    await DisplayAlert(
                        "Login",
                        "E-mail ou senha inválidos.",
                        "OK");

                    return;
                }

                _authService.Login(usuario);

                await DisplayAlert(
                    "Login realizado",
                    $"Bem-vindo, {usuario.Nome}!",
                    "OK");

                // VERIFICA SE O USUÁRIO É ADMINISTRADOR
                if (string.Equals(
                    usuario.TipoUsuario,
                    "Admin",
                    StringComparison.OrdinalIgnoreCase))
                {
                    await Shell.Current.GoToAsync(
                        nameof(AdminPage));
                }
                else
                {
                    await Shell.Current.GoToAsync(
                        nameof(ProdutosPage));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erro",
                    $"Não foi possível conectar à API.\n\n{ex.Message}",
                    "OK");
            }
            finally
            {
                EntrarButton.IsEnabled = true;
            }
        }

        private async void OnCadastroClicked(
            object sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync(
                "CadastroPage");
        }
    }
}