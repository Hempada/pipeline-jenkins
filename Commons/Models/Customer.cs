using System.ComponentModel.DataAnnotations;

namespace Commons.Models
{
    public record Customer(Guid Id, string Name, string Email);

    public record SaveCustomer(
        Guid? Id,
        [Required] string Name,
        [Required] string Email
    );
}
