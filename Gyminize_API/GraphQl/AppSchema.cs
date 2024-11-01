using Gyminize_API.GraphQl.Queries;
using Gyminize_API.GraphQl.Mutations;
using GraphQL.Types;

namespace Gyminize_API.GraphQL
{
    public class AppSchema : Schema
    {
        public AppSchema(CustomerQuery customerQuery, CustomerMutation customerMutation, CustomerhealthQuery customerHealthQuery, CustomerhealthMutations customerHealthMutation)
        {
            // Tạo một ObjectGraphType để gộp các Query
            var combinedQuery = new ObjectGraphType { Name = "Query" };
            combinedQuery.AddField(customerQuery.Fields.Find("customers")); // Giả sử customerQuery có một field "customers"
            combinedQuery.AddField(customerQuery.Fields.Find("customer")); // Giả sử customerQuery có một field "customers"
            combinedQuery.AddField(customerHealthQuery.Fields.Find("customer_health")); // Thêm field từ CustomerHealthQuery
            combinedQuery.AddField(customerHealthQuery.Fields.Find("customer_health_id")); // Thêm field từ CustomerHealthQuery

            this.Query = combinedQuery;

            // Tạo một ObjectGraphType để gộp các Mutation
            var combinedMutation = new ObjectGraphType { Name = "Mutation" };
            combinedMutation.AddField(customerMutation.Fields.Find("addCustomer")); // Thêm field từ CustomerMutation
            combinedMutation.AddField(customerHealthMutation.Fields.Find("addCustomer_health")); // Thêm field từ CustomerHealthMutation

            this.Mutation = combinedMutation;
        }
    }
}
