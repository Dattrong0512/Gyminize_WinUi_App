using GraphQL.Types;
namespace Gyminize_API.GraphQl.Types
{
    public class FooddetailinputType : InputObjectGraphType
    {
        public FooddetailinputType()
        {
            Name = "FooddetailInput";
            Field<NonNullGraphType<IntGraphType>>("dailydiary_id");
            Field<NonNullGraphType<IntGraphType>>("food_id");
            Field<NonNullGraphType<IntGraphType>>("food_amount");
        }
    }
}

