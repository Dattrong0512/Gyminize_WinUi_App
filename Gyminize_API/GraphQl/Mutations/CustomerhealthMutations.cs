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
            Field<Customer_healthType>
                (
                "updateWeightOfCustomer",
                "IsUsed to update weight of a customer",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "customer_id"
                    },
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "weight"
                    }
                    ),
                    resolve: context =>
                    {
                        var customer_id = context.GetArgument<int>("customer_id");
                        var weight = context.GetArgument<int>("weight");
                        if(customer_id != null && weight != null)
                        {
                            return repository.UpdateWeightCustomer(customer_id, weight);
                        }
                        return null;
                    }
                );
        }          
    }
}


