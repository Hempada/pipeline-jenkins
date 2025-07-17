using System.ComponentModel.DataAnnotations;

namespace Commons.Models
{
    public record Account(Guid Id, string Name, string Username, string Email, Profile? Profile);

    public record SaveAccount(
        Guid? Id,
        [Required] string Name,
        [Required] string Username,
        [Required] string Email,
        [Required] Guid ProfileId);

    public record Session(Account Account, string Token);

    public record AuthenticateUser(
        [Required] string UserName,
        [Required] string EncryptedSecret
    );

    public record RegisterUser(
        [Required] string Name,
        [Required] string UserName,
        [Required] string Email,
        [Required] string EncryptedSecret
    );
}
