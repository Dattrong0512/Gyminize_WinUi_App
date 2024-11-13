using GraphQL.Types;

namespace Gyminize_API.GraphQl.Types
{
    public class WorkoutdetailInputType : InputObjectGraphType
    {
        public WorkoutdetailInputType()
        {
            Name = "WorkoutdetailInput";

            Field<NonNullGraphType<IntGraphType>>("typeworkout_id", description: "Id of the type workout associated with this workout detail");
            Field<NonNullGraphType<IntGraphType>>("plan_id", description: "Id of the plan associated with this workout detail");
            Field<NonNullGraphType<DateTimeGraphType>>("date_workout", description: "Date of the workout");
            Field<StringGraphType>("description", description: "Description of the workout detail");
        }
    }
}
