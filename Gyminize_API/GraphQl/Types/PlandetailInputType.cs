using GraphQL.Types;

namespace Gyminize_API.GraphQl.Types
{
    public class PlandetailInputType : InputObjectGraphType
    {
        public PlandetailInputType()
        {
            Name = "PlandetailInput";

            // Các trường đầu vào tương ứng với Plandetail
            Field<NonNullGraphType<IntGraphType>>("plan_id", description: "Id of the plan associated with the plan detail.");
            Field<NonNullGraphType<IntGraphType>>("customer_id", description: "Id of the customer associated with the plan detail.");
            Field<NonNullGraphType<DateTimeGraphType>>("created_at", description: "Creation date of the plan detail.");
            Field<DateTimeGraphType>("start_date", description: "Start date of the plan.");
            Field<DateTimeGraphType>("end_date", description: "End date of the plan.");
        }
    }
}
