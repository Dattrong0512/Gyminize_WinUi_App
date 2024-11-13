using GraphQL.Types;

namespace Gyminize_API.GraphQl.Types
{
    public class PlanInputType : InputObjectGraphType
    {
        public PlanInputType()
        {
            Name = "PlanInput";

            Field<NonNullGraphType<StringGraphType>>("plan_name", description: "Name of the plan");
            Field<StringGraphType>("description", description: "Description of the plan");
            Field<NonNullGraphType<IntGraphType>>("duration_week", description: "Duration of the plan in weeks");
        }
    }
}
