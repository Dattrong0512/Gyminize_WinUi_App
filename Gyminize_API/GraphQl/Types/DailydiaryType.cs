using GraphQL;
using GraphQL.Types;
using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;

namespace Gyminize_API.GraphQl.Type
{
    public class DailydiaryType : ObjectGraphType<Dailydiary>
    {
        public DailydiaryType()
        {
            Field(x => x.customer_id, type: typeof(IntGraphType)).Description("ID of customer");
            Field(x => x.diary_date,type: typeof(DateGraphType)).Description("Date of diary");
            Field(x => x.calories_remain, type: typeof(IntGraphType)).Description("Calories remain");
            Field(x => x.notes, type: typeof(StringGraphType)).Description("Notes");
        }
    }
}


