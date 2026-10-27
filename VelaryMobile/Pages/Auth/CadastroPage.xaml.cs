using VelaryMobile.Models;
using VelaryMobile.Services;

namespace VelaryMobile.Pages.Auth
{
    public partial class CadastroPage : ContentPage
    {
        private readonly ApiService _apiService;

        public CadastroPage()
        {
            InitializeComponent();

            _apiService = new ApiService();
        }

        private async void OnCadastrarClicked(
            object sender,
            EventArgs e)
        {
            MensagemLabel.IsVisible = false;

            if (string.IsNullOrWhiteSpace(NomeEntry.Text) ||
                string.IsNullOrWhiteSpace(EmailEntry.Text) ||
                string.IsNullOrWhiteSpace(SenhaEntry.Text) ||
                string.IsNullOrWhiteSpace(ConfirmarSenhaEntry.Text))
            {
                MensagemLabel.Text =
                    "Preencha todos os campos.";

                MensagemLabel.IsVisible = true;

                return;
            }

            if (SenhaEntry.Text != ConfirmarSenhaEntry.Text)
            {
                MensagemLabel.Text =
                    "As senhas não são iguais.";

                MensagemLabel.IsVisible = true;

                return;
            }

            try
            {
                var usuario = new Usuario
                {
                    Nome = NomeEntry.Text.Trim(),
                    Email = EmailEntry.Text.Trim(),
                    Senha = SenhaEntry.Text,
                    TipoUsuario = "Cliente"
                };

                var resultado =
                    await _apiService.CadastrarUsuarioAsync(
                        usuario);

                if (resultado == null)
                {
                    MensagemLabel.Text =
                        "Não foi possível realizar o cadastro.";

                    MensagemLabel.IsVisible = true;

                    return;
                }

                await DisplayAlert(
                    "Cadastro realizado",
                    "Sua conta foi criada com sucesso!",
                    "OK");

                await Shell.Current.GoToAsync(
                    "LoginPage");
            }
            catch (Exception ex)
            {
                MensagemLabel.Text =
                    $"Erro ao conectar com a API: {ex.Message}";

                MensagemLabel.IsVisible = true;
            }
        }

        private async void OnLoginClicked(
            object sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync(
                "LoginPage");
        }
    }
}