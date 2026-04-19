namespace PIM2026.Services
{
    public static class AuthHelper
    {
        public static bool IsAuthenticated(HttpContext context)
        {
            return context.Session.GetInt32("UsuarioId").HasValue;
        }

        public static int? GetUsuarioId(HttpContext context)
        {
            return context.Session.GetInt32("UsuarioId");
        }

        public static string? GetUsuarioNome(HttpContext context)
        {
            return context.Session.GetString("UsuarioNome");
        }

        public static string? GetUsuarioPerfil(HttpContext context)
        {
            return context.Session.GetString("UsuarioPerfil");
        }

        public static bool IsProfissional(HttpContext context)
        {
            return GetUsuarioPerfil(context) == "Profissional";
        }

        public static bool IsCliente(HttpContext context)
        {
            return GetUsuarioPerfil(context) == "Cliente";
        }

        public static void Logout(HttpContext context)
        {
            context.Session.Clear();
        }
    }
}
