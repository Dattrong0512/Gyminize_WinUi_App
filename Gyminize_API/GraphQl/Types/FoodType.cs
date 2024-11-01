using GraphQL.Server.Types;
using GraphQL.Types;
using Gyminize_API.Data.Model;
namespace Gyminize_API.GraphQl.Types

{
    public class FoodType : ObjectGraphType<Food>
    {
        public FoodType()
        {
            Field(x=>x.food_name,type: typeof(StringGraphType)).Description("Name of the food");
            Field(x => x.calories, type: typeof(IntGraphType)).Description("Calories in the food");
            Field(x => x.protein, type: typeof(DoubleGraphType)).Description("Protein in the food");
            Field(x => x.carbs, type: typeof(DoubleGraphType)).Description("Carbs in the food");
            Field(x => x.fat, type: typeof(DoubleGraphType)).Description("Fat in the food");
        }
    }
}

