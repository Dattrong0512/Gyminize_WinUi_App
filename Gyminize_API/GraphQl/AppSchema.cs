
using Gyminize_API.GraphQl.Queries;
using Gyminize_API.GraphQl.Mutations;
using GraphQL.Types;

namespace Gyminize_API.GraphQL
{
    public class AppSchema : Schema
    {
        public AppSchema(CustomerQuery query, CustomerMutation mutation)
        {
            this.Query = query;
            this.Mutation = mutation;
        }
    }
}
