using VelaryMobile.Services;

namespace VelaryMobile.Pages.Cliente
{
    public partial class PerfilPage : ContentPage
    {
        private readonly AuthService _authService;

        public PerfilPage(AuthService authService)
        {
            InitializeComponent();

            _authService = authService;

            AtualizarPerfil();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AtualizarPerfil();
        }

        private void AtualizarPerfil()
        {
            var usuario = _authService.UsuarioLogado;

            if (usuario == null)
            {
                NomeLabel.Text = "Usuário";
                EmailLabel.Text = "Não identificado";
                return;
            }

            NomeLabel.Text =
                usuario.Nome;

            EmailLabel.Text =
                usuario.Email;
        }

        private async void OnPedidosClicked(
            object sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync(
                nameof(PedidosPage));
        }

        private async void OnProdutosClicked(
            object sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync(
                nameof(ProdutosPage));
        }

        private async void OnSairClicked(
            object sender,
            EventArgs e)
        {
            bool confirmar =
                await DisplayAlert(
                    "Sair",
                    "Deseja realmente sair da sua conta?",
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