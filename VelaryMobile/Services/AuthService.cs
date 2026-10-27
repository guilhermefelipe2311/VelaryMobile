using VelaryMobile.Models;

namespace VelaryMobile.Services
{
    public class AuthService
    {
        private Usuario? _usuarioLogado;

        public Usuario? UsuarioLogado => _usuarioLogado;

        public bool EstaLogado => _usuarioLogado != null;

        public void Login(Usuario usuario)
        {
            _usuarioLogado = usuario;

            Preferences.Set("UsuarioId", usuario.Id);
            Preferences.Set("UsuarioNome", usuario.Nome);
            Preferences.Set("UsuarioEmail", usuario.Email);
            Preferences.Set("UsuarioTipo", usuario.TipoUsuario);
        }

        public void Logout()
        {
            _usuarioLogado = null;

            Preferences.Remove("UsuarioId");
            Preferences.Remove("UsuarioNome");
            Preferences.Remove("UsuarioEmail");
            Preferences.Remove("UsuarioTipo");
        }
    }
}