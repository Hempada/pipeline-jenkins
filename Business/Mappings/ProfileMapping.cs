using Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Business.Mappings
{
    public class ProfileMapping : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder
                .ToTable("Profile")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .Property(x => x.Active)
                .IsRequired();

            builder
                .Ignore(x => x.Permissions)
                .Property<IEnumerable<string>>("Permissions")
                .HasArrayToJsonConversion()
                .IsRequired();
        }
    }
}
