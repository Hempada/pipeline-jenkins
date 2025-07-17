using WebApi.Controllers;

namespace WebApi.Extensions
{
    public static class AuthControllerExtensions
    {
        // Método de extensão para definir o ID da conta atual
        public static void SetCurrentAccountId(this AuthController controller, Guid accountId)
        {
            // Aqui, você pode usar reflexão para acessar o campo privado
            var fieldInfo = typeof(AuthController).GetField("_currentAccountId",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (fieldInfo != null)
            {
                fieldInfo.SetValue(controller, accountId);
            }
        }
    }
}
