using Humanizer;
using Microsoft.OData.ModelBuilder;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Microsoft.AspNetCore.OData;

public static class ODataMappingExtensions
{
    internal static readonly ConditionalWeakTable<object, EntityTypeConfigurationExternalProperties> Data = new ConditionalWeakTable<object, EntityTypeConfigurationExternalProperties>();

    public static ODataOptions AddRouteComponentsFromAssembly(this ODataOptions options, string routePrefix, Assembly assembly)
    {
        ODataConventionModelBuilder oDataConventionModelBuilder = new ODataConventionModelBuilder();
        foreach (Type item in (IEnumerable<Type>)(from x in assembly.GetTypes()
                                                  where Array.Exists(x.GetInterfaces(), (Type i) => i.Name == typeof(IODataTypeConfiguration<>).Name)
                                                  select x).ToArray())
        {
            object obj = Activator.CreateInstance(item)!;
            Type type = obj.GetType();
            Type type2 = type.GetTypeInfo().ImplementedInterfaces.First().GenericTypeArguments[0];
            object obj2 = oDataConventionModelBuilder.GetType().GetMethod("EntityType")?.MakeGenericMethod(type2)
                .Invoke(oDataConventionModelBuilder, null)!;
            type.GetMethod("Configure")?.Invoke(obj, new object[1] { obj2 });
            string? text = (string?)typeof(ODataMappingExtensions).GetMethod("GetNameOrDefault", BindingFlags.Static | BindingFlags.NonPublic)?.MakeGenericMethod(type2).Invoke(null, new object[1] { obj2 });
            if (text == null)
            {
                text = type2.Name.Pluralize().ToLower();
            }

            MethodInfo methodInfo = oDataConventionModelBuilder.GetType().GetMethod("EntitySet")?.MakeGenericMethod(type2)!;
            object[] parameters = new string[1] { text };
            methodInfo.Invoke(oDataConventionModelBuilder, parameters);
        }

        options.AddRouteComponents(routePrefix, oDataConventionModelBuilder.GetEdmModel());
        return options;
    }

    public static EntityTypeConfiguration<TEntity> WithName<TEntity>(this EntityTypeConfiguration<TEntity> configuration, string name) where TEntity : class
    {
        Data.GetOrCreateValue(configuration).Name = name;
        return configuration;
    }

    internal class EntityTypeConfigurationExternalProperties
    {
        public string Name { get; set; } = string.Empty;

    }

    public interface IODataTypeConfiguration<TEntity> where TEntity : class
    {
        void Configure(EntityTypeConfiguration<TEntity> builder);
    }
}