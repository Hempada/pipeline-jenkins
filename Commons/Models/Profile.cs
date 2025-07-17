using System.ComponentModel.DataAnnotations;

namespace Commons.Models
{
    public record Profile(Guid Id, string Name, IEnumerable<string> Permissions);

    public record SaveProfile(
        Guid? Id,
        [Required] string Name,
        [Required] IEnumerable<string> Permissions
    );
}
