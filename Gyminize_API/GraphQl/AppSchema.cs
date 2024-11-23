using Gyminize_API.GraphQl.Queries;
using Gyminize_API.GraphQl.Mutations;
using GraphQL.Types;
using Gyminize_API.Data.Model;

namespace Gyminize_API.GraphQL
{
    public class AppSchema : Schema
    {
        public AppSchema(CustomerQuery customerQuery, CustomerMutation customerMutation, CustomerhealthQuery customerHealthQuery, CustomerhealthMutations customerHealthMutation,
            DailydiaryQuery dailydiaryQuery, DailydiaryMutations dailydiaryMutations, FoodQueries foodQueries, PlandetailQuery plandetailQuery, PlandetailMutation plandetailMutation,
            WorkoutdetailMutation workoutdetailMutation)
        {
            // Tạo một ObjectGraphType để gộp các Query
            var combinedQuery = new ObjectGraphType { Name = "Query" };
            combinedQuery.AddField(customerQuery.Fields.Find("customer_id")); // Giả sử customerQuery có một field "customers"
            combinedQuery.AddField(customerQuery.Fields.Find("customer_username")); // Giả sử customerQuery có một field "customers"
            combinedQuery.AddField(customerHealthQuery.Fields.Find("customer_health_id")); // Thêm field từ CustomerHealthQuery
            combinedQuery.AddField(dailydiaryQuery.Fields.Find("dailydiaries")); 
            combinedQuery.AddField(dailydiaryQuery.Fields.Find("dailydiary")); 
            combinedQuery.AddField(plandetailQuery.Fields.Find("GetplandetailById"));
            combinedQuery.AddField(foodQueries.Fields.Find("foods"));
            this.Query = combinedQuery;

            // Tạo một ObjectGraphType để gộp các Mutation
            var combinedMutation = new ObjectGraphType { Name = "Mutation" };
            combinedMutation.AddField(customerMutation.Fields.Find("addCustomer")); // Thêm field từ CustomerMutation
            combinedMutation.AddField(customerMutation.Fields.Find("updatePasswordCustomer")); // Thêm field từ CustomerMutation
            combinedMutation.AddField(customerHealthMutation.Fields.Find("addCustomer_health")); // Thêm field từ CustomerHealthMutation
            combinedMutation.AddField(customerHealthMutation.Fields.Find("updateWeightOfCustomer")); // Thêm field từ CustomerHealthMutation
            combinedMutation.AddField(dailydiaryMutations.Fields.Find("addDailydiary"));
            combinedMutation.AddField(plandetailMutation.Fields.Find("addPlandetail"));
            combinedMutation.AddField(workoutdetailMutation.Fields.Find("updateDecriptionsWorkoutdetail"));

            this.Mutation = combinedMutation;
        }
    }
}
