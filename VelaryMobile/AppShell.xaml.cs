using VelaryMobile.Pages.Admin;
using VelaryMobile.Pages.Auth;
using VelaryMobile.Pages.Cliente;

namespace VelaryMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // AUTENTICAÇÃO
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(CadastroPage), typeof(CadastroPage));

            // CLIENTE
            Routing.RegisterRoute(nameof(ProdutosPage), typeof(ProdutosPage));
            Routing.RegisterRoute(nameof(CarrinhoPage), typeof(CarrinhoPage));
            Routing.RegisterRoute(nameof(PedidosPage), typeof(PedidosPage));
            Routing.RegisterRoute(nameof(PerfilPage), typeof(PerfilPage));

            // ADMINISTRADOR
            Routing.RegisterRoute(nameof(AdminPage), typeof(AdminPage));
            Routing.RegisterRoute(nameof(AdminProdutosPage), typeof(AdminProdutosPage));
            Routing.RegisterRoute(nameof(AdminPedidosPage), typeof(AdminPedidosPage));
            Routing.RegisterRoute(nameof(AdminUsuariosPage), typeof(AdminUsuariosPage));
        }
    }
}