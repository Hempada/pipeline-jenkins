using Business.Models;
using Microsoft.OData.ModelBuilder;
using static Microsoft.AspNetCore.OData.ODataMappingExtensions;

namespace WebApi.ODataMappings
{
    public class AccountODataMapping : IODataTypeConfiguration<Account>
    {
        public void Configure(EntityTypeConfiguration<Account> builder)
        {
            builder.WithName("accounts");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name);
            builder.Property(x => x.Username);
            builder.Property(x => x.Email);
            builder.Property(x => x.ProfileId);
        }
    }

}
