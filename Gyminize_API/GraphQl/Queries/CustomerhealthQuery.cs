using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Types;
using GraphQL;
using GraphQL.Types;

namespace Gyminize_API.GraphQl.Queries
{
    public class CustomerhealthQuery : ObjectGraphType
    {
        public CustomerhealthQuery(CustomerHealthRepository repository)
        {
            Field<ListGraphType<Customer_healthType>>(
                "customer_health",
                resolve: context => repository.GetAllCustomerHealth()
            );
            Field<Customer_healthType>(
                "customer_health_id",
                description: "Return a customer_health by id",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "customer_id"
                    }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("customer_id");
                    return repository.GetCustomerHealthByCustomerId(id);
                }
                );
        }
    }

}


