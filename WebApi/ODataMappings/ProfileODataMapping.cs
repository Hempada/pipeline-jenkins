using Business.Models;
using Microsoft.OData.ModelBuilder;
using static Microsoft.AspNetCore.OData.ODataMappingExtensions;

namespace WebApi.ODataMappings
{
    public class ProfileODataMapping : IODataTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeConfiguration<Profile> builder)
        {
            builder.WithName("profiles");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name);
            builder.CollectionProperty(x => x.Permissions);
        }
    }

}
