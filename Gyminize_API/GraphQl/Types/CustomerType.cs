using GraphQL.Types;
using Gyminize_API.Data.Models;

namespace Gyminize_API.GraphQl.Types
{
    public class CustomerType : ObjectGraphType<Customer>
    {
        public CustomerType()
        {
            Field(x => x.customer_name, type: typeof(StringGraphType)).Description("Name of customer");
            Field(x => x.auth_type, type: typeof(IntGraphType)).Description("Auth");
            Field(x => x.username, type: typeof(StringGraphType)).Description("Username of customer");
            Field(x => x.customer_password, type: typeof(StringGraphType)).Description("Password of customer");
        }
    }
}
