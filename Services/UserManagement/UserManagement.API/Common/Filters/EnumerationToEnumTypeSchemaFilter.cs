using UserManagement.Domain.SeedWork;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace UserManagement.API.Common.Filters
{
    public class EnumerationToEnumTypeSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsSubclassOf(typeof(Enumeration)))
            {
                IList<string> fields = context.Type.GetFields(BindingFlags.Public |
                                        BindingFlags.Static |
                                        BindingFlags.DeclaredOnly).Select(x => x.Name).ToList();

                IList<string> properties = context.Type.GetProperties(BindingFlags.Public |
                         BindingFlags.Static |
                         BindingFlags.DeclaredOnly).Select(x => x.Name).ToList();

                IList<string> enums = fields.Concat(properties).ToList();

                schema.Enum = enums.Select(x => new OpenApiString(x)).Cast<IOpenApiAny>().ToList();
                schema.Type = nameof(String);
                schema.Properties = null;
                schema.AllOf = null;
            }
        }
    }
}
