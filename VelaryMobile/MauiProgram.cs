using Microsoft.Extensions.Logging;
using VelaryMobile.Pages.Admin;
using VelaryMobile.Pages.Auth;
using VelaryMobile.Pages.Cliente;
using VelaryMobile.Services;

namespace VelaryMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont(
                        "OpenSans-Regular.ttf",
                        "OpenSansRegular");

                    fonts.AddFont(
                        "OpenSans-Semibold.ttf",
                        "OpenSansSemibold");
                });

            // SERVIÇOS

            builder.Services.AddSingleton<CarrinhoService>();
            builder.Services.AddSingleton<AuthService>();


            // AUTENTICAÇÃO

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<CadastroPage>();


            // CLIENTE

            builder.Services.AddTransient<ProdutosPage>();
            builder.Services.AddTransient<CarrinhoPage>();
            builder.Services.AddTransient<PedidosPage>();
            builder.Services.AddTransient<PerfilPage>();


            // ADMINISTRADOR

            builder.Services.AddTransient<AdminPage>();
            builder.Services.AddTransient<AdminProdutosPage>();
            builder.Services.AddTransient<AdminPedidosPage>();
            builder.Services.AddTransient<AdminUsuariosPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}