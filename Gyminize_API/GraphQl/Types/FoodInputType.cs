using GraphQL.Server.Types;
using GraphQL.Types;
namespace Gyminize_API.GraphQl.Types
{
    public class FoodInputType : InputObjectGraphType
    {
        public FoodInputType()
        {
            Name = "FoodInput";
            Field<NonNullGraphType<StringGraphType>>("food_name");
            Field<NonNullGraphType<IntGraphType>>("calories");
            Field<NonNullGraphType<DoubleGraphType>>("protein");
            Field<NonNullGraphType<DoubleGraphType>>("carbs");
            Field<NonNullGraphType<DoubleGraphType>>("fats");
          
        }
    }
}


