using VelaryMobile.Pages.Auth;
using VelaryMobile.Pages.Cliente;

namespace VelaryMobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnEntrarClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }

        private async void OnCadastroClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CadastroPage));
        }

        private async void OnProdutosClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ProdutosPage));
        }
    }
}