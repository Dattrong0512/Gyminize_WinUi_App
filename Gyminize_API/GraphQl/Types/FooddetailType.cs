using GraphQL.Types;
using Gyminize_API.Data.Model;
namespace Gyminize_API.GraphQl.Types
{
    public class FooddetailType :ObjectGraphType<Fooddetail>
    {
        public FooddetailType()
        {
            Field(x => x.dailydiary_id, type: typeof(IntGraphType)).Description("Id of daily diary");
            Field(x => x.food_id,type: typeof(IntGraphType)).Description("Id of food");
            Field(x => x.food_amount, type: typeof(IntGraphType)).Description("Total food");
        }
    }
}


