
using GraphQL;
using GraphQL.Types;
using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Types;
namespace Gyminize_API.GraphQl.Mutations
{
    public class CustomerMutation : ObjectGraphType
    {
        public CustomerMutation(CustomerRepository repository)
        {
            Field<CustomerType>(
                "addCustomer",
                "IsUsed to add a new customer",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CustomerInputType>> { Name = "Customer" }
                ),
                resolve: context =>
                {
                    var customer = context.GetArgument<Customer>("customer");
                    if (customer != null)
                    {
                        return repository.addCustomer(customer);
                    }
                    return null;
                });
        }

    }
}
