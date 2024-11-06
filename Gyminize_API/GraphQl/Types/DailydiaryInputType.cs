using GraphQL.Types;
namespace Gyminize_API.GraphQl.Types
{
    public class DailydiaryInputType : InputObjectGraphType
    {
        public DailydiaryInputType() {
            Name = "dailydiaryInput";
            Field<IntGraphType>("customer_id");
            Field<NonNullGraphType<DateGraphType>>("diary_date");
            Field<NonNullGraphType<IntGraphType>>("calories_remain");
            Field<NonNullGraphType<IntGraphType>>("daily_weight");
            Field<NonNullGraphType<DecimalGraphType>>("total_calories");
            Field<StringGraphType>("notes");
            Field<FooddetailType>("fooddetails");
        }    
    }
}


