﻿using Gyminize_API.GraphQl.Queries;
using Gyminize_API.GraphQl.Mutations;
using GraphQL.Types;
using Gyminize_API.Data.Model;

namespace Gyminize_API.GraphQL
{
    public class AppSchema : Schema
    {
        public AppSchema(CustomerQuery customerQuery, CustomerMutation customerMutation, CustomerhealthQuery customerHealthQuery, CustomerhealthMutations customerHealthMutation,
            DailydiaryQuery dailydiaryQuery, FooddetailQuery fooddetailQuery,FooddetailMutations fooddetailMutations ,FoodQueries foodQueries)
        {
            // Tạo một ObjectGraphType để gộp các Query
            var combinedQuery = new ObjectGraphType { Name = "Query" };
            combinedQuery.AddField(customerQuery.Fields.Find("customers")); // Giả sử customerQuery có một field "customers"
            combinedQuery.AddField(customerQuery.Fields.Find("customer")); // Giả sử customerQuery có một field "customers"
            combinedQuery.AddField(customerHealthQuery.Fields.Find("customer_health")); // Thêm field từ CustomerHealthQuery
            combinedQuery.AddField(customerHealthQuery.Fields.Find("customer_health_id")); // Thêm field từ CustomerHealthQuery
            combinedQuery.AddField(dailydiaryQuery.Fields.Find("dailydiaries")); 
            combinedQuery.AddField(dailydiaryQuery.Fields.Find("dailydiary")); 
            combinedQuery.AddField(fooddetailQuery.Fields.Find("fooddetails"));
            combinedQuery.AddField(fooddetailQuery.Fields.Find("fooddetail")); 
            //combinedQuery.AddField(foodQueries.Fields.Find("foodDetailsByCustomer"));
            combinedQuery.AddField(foodQueries.Fields.Find("foods"));
            combinedQuery.AddField(foodQueries.Fields.Find("food"));
            this.Query = combinedQuery;

            // Tạo một ObjectGraphType để gộp các Mutation
            var combinedMutation = new ObjectGraphType { Name = "Mutation" };
            combinedMutation.AddField(customerMutation.Fields.Find("addCustomer")); // Thêm field từ CustomerMutation
            combinedMutation.AddField(customerHealthMutation.Fields.Find("addCustomer_health")); // Thêm field từ CustomerHealthMutation
            combinedMutation.AddField(fooddetailMutations.Fields.Find("addOrUpdateFooddetail"));

            this.Mutation = combinedMutation;
        }
    }
}
