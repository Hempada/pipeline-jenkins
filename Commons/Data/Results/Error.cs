namespace Commons.Data.Results
{
    public class Error
    {
        public string Code { get; init; }

        public string Description { get; init; }

        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }

        #region Generic
        public static Error EmptyRequiedFields => new Error("EMPTY_REQUIRED_FIELDS", "Campos obrigatórios não informados.");
        public static Error SaveChangesFailed => new Error("SAVE_CHANGES_FAILED", "Erro ao manipular a base de dados.");
        #endregion Generic

        #region Auth
        public static Error AccountNotFound => new Error("ACCOUNT_NOT_FOUND", "Usuário não encontrado.");
        public static Error UsernameAlreadyExists => new Error("USERNAME_ALREADY_EXISTS", "Usuário já cadastrado.");
        public static Error EmailAlreadyExists => new Error("EMAIL_ALREADY_EXISTS", "E-mail já cadastrado.");
        public static Error RegisterAuthFailed => new Error("REGISTER_AUTH_FAILED", "Não foi possível registrar o usuário no serviço de autenticação.");
        #endregion Auth

        #region Profile
        public static Error ProfileAlreadyExists => new Error("PROFILE_ALREADY_EXISTS", "Perfil já cadastrado.");
        public static Error ProfileNotFound => new Error("PROFILE_NOT_FOUND", "Perfil não encontrado.");
        public static Error ProfileAssociated => new Error("PROFILE_ASSOCIATED", "O Perfil está associado a um ou mais Usuários e não pode ser excluído.");
        #endregion Profile

        #region Customer
        public static Error CustomerAlreadyExists => new Error("CUSTOMER_ALREADY_EXISTS", "Cliente já cadastrado.");
        public static Error CustomerNotFound => new Error("CUSTOMER_NOT_FOUND", "Cliente não encontrado.");
        #endregion Customer
    }
}
