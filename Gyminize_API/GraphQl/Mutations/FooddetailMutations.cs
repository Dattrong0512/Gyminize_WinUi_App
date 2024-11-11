using GraphQL;
using GraphQL.Types;
using Gyminize_API.Data.Model;
using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Types;

namespace Gyminize_API.GraphQl.Mutations
{
    public class FooddetailMutations : ObjectGraphType
    {
        public FooddetailMutations(FooddetailRepository fooddetailRepository)
        {
            Field<FooddetailType>(
                "addOrUpdateFooddetail",
                "Is used to add or update a fooddetail with associated food details",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<FooddetailinputType>> { Name = "fooddetail" }
                ),
                resolve: context =>
                {
                    var fooddetail = context.GetArgument<Fooddetail>("fooddetail");

                    if (fooddetail != null)
                    {
                        // Gọi repository để thêm hoặc cập nhật Fooddetail
                        return fooddetailRepository.AddOrUpdateFooddetail(
                            fooddetail.dailydiary_id,
                            fooddetail.meal_type,
                            fooddetail.Food,
                            fooddetail.food_amount
                        );
                    }
                    return null;
                });
            Field<FooddetailType>(
                "DeleteFoodFromFooddetail",
                "Is used to delete fooddetail with associated food details",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<FooddetailinputType>> { Name = "fooddetail" }
                ),
                resolve: context =>
                {
                    var fooddetail = context.GetArgument<Fooddetail>("fooddetail");

                    if (fooddetail != null)
                    {
                        // Gọi repository để thêm hoặc cập nhật Fooddetail
                        return fooddetailRepository.DeleteFoodFromFooddetail(
                            fooddetail.dailydiary_id,
                            fooddetail.meal_type,
                            fooddetail.Food
                        );
                    }
                    return null;
                });
        }
    }
}
