using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business.Mappings
{
    public class AccountMapping : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder
                .ToTable("Account")
                .HasKey(x => x.Id);

            builder
              .Property(x => x.Name)
              .IsRequired();

            builder
                .Property(x => x.Username)
                .IsRequired();

            builder
                .Property(x => x.Email)
                .IsRequired();

            builder
                .Property(x => x.Active)
                .IsRequired();

            builder
                 .Property(x => x.ProfileId)
                 .IsRequired(false);
        }
    }
}
