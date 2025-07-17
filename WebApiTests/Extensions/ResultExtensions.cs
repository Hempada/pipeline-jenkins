using Commons.Data.Results;

namespace WebApiTests.Extensions
{
    public static class ResultExtensions
    {
        // Verifica se o resultado foi bem-sucedido (não contém erros)
        public static bool IsSuccess(this Result result)
        {
            return result.Valid;
        }

        public static bool IsSuccess<T>(this Result<T> result)
        {
            return result.Valid;
        }

        // Retorna os dados do Result<T> se bem-sucedido
        public static T? Value<T>(this Result<T> result)
        {
            return result.Data; // Certifique-se de que 'Data' é uma propriedade pública
        }

        // Converte Result<T> para Result mantendo os erros
        public static Result ToResult<T>(this Result<T> result)
        {
            var errors = result.GetErrors(); // Agora você pode chamar GetErrors para Result<T>
            return Result.Fail(errors); // Ou use o método apropriado para criar um novo resultado
        }

        // Obtém a lista de erros de um Result
        public static IEnumerable<Error> GetErrors(this Result result)
        {
            return result.Errors; // Acesse a lista de erros
        }

        // Obtém a lista de erros de um Result<T>
        public static IEnumerable<Error> GetErrors<T>(this Result<T> result)
        {
            return result.Errors; // Acesse a lista de erros no Result<T>
        }
    }
}
