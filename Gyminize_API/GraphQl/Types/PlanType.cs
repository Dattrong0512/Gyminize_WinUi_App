using Gyminize_API.Data.Model;
using GraphQL.Types;

namespace Gyminize_API.GraphQl.Types
{
    public class PlanType : ObjectGraphType<Plan>
    {
        public PlanType()
        {
            Field(x => x.plan_id, type: typeof(IntGraphType)).Description("Id of the plan");
            Field(x => x.plan_name).Description("Name of the plan");
            Field(x => x.description).Description("Description of the plan");
            Field(x => x.duration_week, type: typeof(IntGraphType)).Description("Duration of the plan in weeks");

            // Thêm trường liên kết đến Workoutdetails
            Field<ListGraphType<WorkoutdetailType>>(
                "workoutdetails",
                resolve: context => context.Source.Workoutdetails,
                description: "List of workout details associated with the plan"
            );
        }
    }
}
