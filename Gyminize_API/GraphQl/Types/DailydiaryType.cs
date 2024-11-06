using GraphQL;
using GraphQL.Types;
using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Gyminize_API.GraphQl.Types;

namespace Gyminize_API.GraphQl.Type
{
    public class DailydiaryType : ObjectGraphType<Dailydiary>
    {
        public DailydiaryType()
        {
            Field(x => x.customer_id, type: typeof(IntGraphType)).Description("ID of customer");
            Field(x => x.diary_date,type: typeof(DateGraphType)).Description("Date of diary");
            Field(x => x.calories_remain, type: typeof(IntGraphType)).Description("Calories remain");
            Field(x=> x.daily_weight, type: typeof(IntGraphType)).Description("Daily weight");
            Field(x => x.total_calories, type: typeof(DecimalGraphType)).Description("Total calories");
            Field(x => x.notes, type: typeof(StringGraphType)).Description("Notes");
            Field(x=> x.Fooddetails, type: typeof(ListGraphType<FooddetailType>)).Description("Food details");
            Field<FooddetailType>(
                "fooddetails",
                resolve: context => context.Source.Fooddetails,
                description: "The associated fooddetails object"
            );
        }
    }
}


