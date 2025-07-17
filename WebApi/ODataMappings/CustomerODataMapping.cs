using Business.Models;
using Microsoft.OData.ModelBuilder;
using static Microsoft.AspNetCore.OData.ODataMappingExtensions;

namespace WebApi.ODataMappings
{
    public class CustomerODataMapping : IODataTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeConfiguration<Customer> builder)
        {
            builder.WithName("customers");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name);
            builder.Property(x => x.Email);
        }
    }

}
