using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Types;
using GraphQL;
using GraphQL.Types;
using System.ComponentModel.DataAnnotations;
using GraphQL.Resolvers;
using Gyminize_API.Data.Models;

namespace Gyminize_API.GraphQl.Queries
{
    public class CustomerQuery : ObjectGraphType
    {
        public CustomerQuery(CustomerRepository repository)
        {

            // Truy vấn trả về một khách hàng dựa trên ID
            Field<CustomerType>(
                "customer_username",
                description: "Return a customer by username",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "username" }
                ),
                resolve: context =>
                {
                    var username = context.GetArgument<String>("username");
                    return repository.GetCustomerByUsername(username);
                }
            );
            // Truy vấn trả về một khách hàng dựa trên ID
            Field<CustomerType>(
                "customer_id",
                description: "Return a customer by id",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "customer_id" }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("customer_id");
                    return repository.GetCustomerById(id);
                }
            );
        }
    }
}

