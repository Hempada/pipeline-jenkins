namespace Commons.Models
{
    public static class Permission
    {
        public const string VIEW_ACCOUNT = "VIEW_ACCOUNT";
        public const string CREATE_ACCOUNT = "CREATE_ACCOUNT";
        public const string DELETE_ACCOUNT = "DELETE_ACCOUNT";

        public const string VIEW_PROFILE = "VIEW_PROFILE";
        public const string CREATE_PROFILE = "CREATE_PROFILE";
        public const string DELETE_PROFILE = "DELETE_PROFILE";

        public const string VIEW_CUSTOMER = "VIEW_CUSTOMER";
        public const string CREATE_CUSTOMER = "CREATE_CUSTOMER";
        public const string DELETE_CUSTOMER = "DELETE_CUSTOMER";

        public static IEnumerable<string> All => [
            VIEW_ACCOUNT, CREATE_ACCOUNT, DELETE_ACCOUNT,
            VIEW_PROFILE, CREATE_PROFILE, DELETE_PROFILE,
            VIEW_CUSTOMER, CREATE_CUSTOMER, DELETE_CUSTOMER,
        ];

        public static string GetDescription(string permission)
        {
            switch (permission)
            {
                case VIEW_ACCOUNT: return "Visualizar Contas";
                case CREATE_ACCOUNT: return "Criar/Editar Conta";
                case DELETE_ACCOUNT: return "Excluir Conta";

                case VIEW_PROFILE: return "Visualizar Perfis";
                case CREATE_PROFILE: return "Criar/Editar Perfil";
                case DELETE_PROFILE: return "Excluir Perfil";

                case VIEW_CUSTOMER: return "Visualizar Clientes";
                case CREATE_CUSTOMER: return "Criar/Editar Cliente";
                case DELETE_CUSTOMER: return "Excluir Cliente";

                default: return string.Empty;
            }
        }

        public static IEnumerable<string> GetDescription(IEnumerable<string> permissions)
        {
            foreach (string permission in permissions)
            {
                yield return GetDescription(permission);
            }
        }
    }
}
