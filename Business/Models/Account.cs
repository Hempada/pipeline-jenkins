namespace Business.Models
{
    public class Account
    {
        public Account()
        {
        }

        #region Properties
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Username { get; set; } = default!;

        public string Email { get; set; } = default!;

        public bool Active { get; set; }

        public Guid? ProfileId { get; set; }

        #endregion Properties

        #region Properties Navigation

        public Profile? Profile { get; set; }

        #endregion Properties Navigation

        public static Account Create(string name, string username, string email, Guid? profileId)
        {
            return new()
            {
                Name = name,
                Username = username,
                Email = email,
                ProfileId = profileId
            };
        }

        public void Update(string name, string username, string email, Guid? profileId)
        {
            Name = name;
            Username = username;
            Email = email;
            ProfileId = profileId;
        }

        public Commons.Models.Account ToCommons()
        {
            return new Commons.Models.Account(
                Id,
                Name,
                Username,
                Email,
                Profile?.ToCommons());
        }
    }
}