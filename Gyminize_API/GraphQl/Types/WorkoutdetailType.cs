using Gyminize_API.Data.Model;
using GraphQL.Types;

namespace Gyminize_API.GraphQl.Types
{
    public class WorkoutdetailType : ObjectGraphType<Workoutdetail>
    {
        public WorkoutdetailType()
        {
            Field(x => x.workoutdetail_id, type: typeof(IntGraphType)).Description("Id of the workout detail");
            Field(x => x.typeworkout_id, type: typeof(IntGraphType)).Description("Id of the type workout");
            Field(x => x.plan_id, type: typeof(IntGraphType)).Description("Id of the plan");
            Field(x => x.date_workout, type: typeof(DateTimeGraphType)).Description("Date of the workout");
            Field(x => x.description).Description("Description of the workout detail");

            // Thêm trường liên kết đến Typeworkout
            Field<TypeworkoutType>(
                "typeworkout",
                resolve: context => context.Source.Typeworkout,
                description: "The associated type workout object"
            );
        }
    }
}
