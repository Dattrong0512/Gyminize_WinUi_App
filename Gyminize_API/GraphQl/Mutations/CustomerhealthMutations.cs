using GraphQL;
using GraphQL.Types;
using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Types;
namespace Gyminize_API.GraphQl.Mutations
{
    public class CustomerhealthMutations : ObjectGraphType
    {
        public CustomerhealthMutations(CustomerHealthRepository repository) {
            Field<Customer_healthType>
                (
                "addCustomer_health",
                "IsUsed to add a new customer_health",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<Customer_healthInputType>>
                    {
                        Name = "Customer_health"
                    }
                    ),
                resolve: context =>
                {
                    var customer_health = context.GetArgument<Customer_health>("customer_health");
                    if (customer_health != null)
                    {
                        return repository.AddCustomerHealth(customer_health);
                    }
                    return null;
                }
                );
        }          
    }
}


