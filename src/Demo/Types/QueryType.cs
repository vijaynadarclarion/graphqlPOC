using System.Security.Claims;
using Demo.Data;
using Demo.Dataloader;
using Demo.Entity;
using Demo.Extensions;
using Demo.Resolvers;
using Microsoft.EntityFrameworkCore;
using NUPDemo.Entities;

namespace Demo.Types
{
    public interface ISchemaLoader
    {
        Task<ISchema> LoadSchemaAsync(HttpRequest request);
    }

    public class MySchemaLoader : ISchemaLoader
    {
        public async Task<ISchema> LoadSchemaAsync(HttpRequest request)
        {
            // Logic to determine which schema to load based on the request
            string schemaName = DetermineSchemaName(request);

            // Load the appropriate schema based on the schema name
            ISchema schema = await LoadSchemaFromName(schemaName);

            return schema;
        }

        private string DetermineSchemaName(HttpRequest request)
        {
            // Implement logic to determine the schema name based on the request
            return request.Path.Value.Contains("admin") ? "AdminSchema" : "DefaultSchema";
        }

        private async Task<ISchema> LoadSchemaFromName(string schemaName)
        {
            // Implement logic to load the schema based on the schema name
            // Example: Load schema from a file or a database
            return null;
        }
    }

   

    public class QueryType : ObjectType<Query>
    {

        public QueryType()
        {
        }

        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Description("Represents any insurance company information.");

            descriptor
                .Field("accidents")
                .Argument("caseNumbers", argumentDescriptor =>
                     argumentDescriptor.Type<ListType<StringType>>())
                .ResolveWith<TestResolver>(p => p.GetAccident(default!))
                //.UseScopedDbContext<AppReadOnlyDbContext>()
                //.UseDbContext< AppReadOnlyDbContext>()
                .Description("Represents the accident information.");         

        }
    }
}
