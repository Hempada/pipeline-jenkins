namespace Business.Models
{
    public class Customer
    {
        public Customer()
        {
        }

        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }


        public static Customer Create(string name, string email)
        {
            return new()
            {
                Name = name,
                Email = email
            };
        }

        public void Update(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
