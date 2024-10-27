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
            // Truy vấn trả về danh sách khách hàng
            Field<ListGraphType<CustomerType>>(
                "customers",
                description: "Return all customers",
                resolve: context => repository.GetAllCustomer()
            );

            // Truy vấn trả về một khách hàng dựa trên ID
            Field<CustomerType>(
                "customer",
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

