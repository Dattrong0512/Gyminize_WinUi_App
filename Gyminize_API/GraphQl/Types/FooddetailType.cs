using Gyminize_API.Data.Model;
using GraphQL.Types;

namespace Gyminize_API.GraphQl.Types
{
    public class FooddetailType : ObjectGraphType<Fooddetail>
    {
        public FooddetailType()
        {
            Field(x => x.fooddetail_id, type: typeof(IntGraphType)).Description("Id of food detail");
            Field(x => x.dailydiary_id, type: typeof(IntGraphType)).Description("Id of daily diary");
            Field(x => x.food_id, type: typeof(IntGraphType)).Description("Id of food");
            Field(x => x.meal_type, type: typeof(IntGraphType)).Description("Meal type");
            Field(x => x.food_amount, type: typeof(IntGraphType)).Description("Total food");

            // Thêm trường Food để trả về thông tin chi tiết của đối tượng Food
            Field<FoodType>(
                "food",
                resolve: context => context.Source.Food,
                description: "The associated food object"
            );
        }
    }
}
