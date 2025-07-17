namespace Business.Models
{
    public class Profile
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public bool Active { get; set; }

        public IEnumerable<string> Permissions { get; set; } = default!;

        public static Profile Create(string name, IEnumerable<string> permissions)
        {
            return new()
            {
                Name = name,
                Permissions = permissions,
                Active = true
            };
        }

        public void Update(string name, IEnumerable<string> permissions)
        {
            Name = name;
            Permissions = permissions;
        }

        public Commons.Models.Profile ToCommons()
        {
            return new Commons.Models.Profile(
                Id,
                Name,
                Permissions);
        }
    }
}