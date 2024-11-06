using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Types;
using GraphQL;
using GraphQL.Types;

namespace Gyminize_API.GraphQl.Queries
{
    public class FooddetailQuery : ObjectGraphType
    {
        public FooddetailQuery(FooddetailRepository repository)
        {
            Field<ListGraphType<FooddetailType>>(
                "fooddetails",
                resolve: context => repository.GetAllFooddetail()
            );

            Field<FooddetailType>(
                "fooddetail",
                description: "Return a fooddetail by id",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "food_id" }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("food_id");
                    return repository.GetFooddetailById(id);
                }
            );

            //Field<ListGraphType<FooddetailType>>(
            //    "foodDetailsByCustomer",
            //    description: "Return food details by customer ID for a specific date",
            //    arguments: new QueryArguments(
            //        new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "customer_id" }
            //    ),
            //    resolve: context =>
            //    {
            //        var customerId = context.GetArgument<int>("customer_id");
            //        return repository.GetFoodDetailsByCustomerId(customerId);
            //    }
            //);
        }
    }
}
