using Gyminize_API.Data.Model;
using GraphQL.Types;

namespace Gyminize_API.GraphQl.Types
{
    public class PlandetailType : ObjectGraphType<Plandetail>
    {
        public PlandetailType()
        {
            Field(x => x.plandetail_id, type: typeof(IntGraphType)).Description("Id of plan detail");
            Field(x => x.plan_id, type: typeof(IntGraphType)).Description("Id of plan");
            Field(x => x.customer_id, type: typeof(IntGraphType)).Description("Id of customer");
            Field(x => x.created_at, type: typeof(DateTimeGraphType)).Description("Creation date of the plan detail");
            Field(x => x.start_date, type: typeof(DateTimeGraphType)).Description("Start date of the plan");
            Field(x => x.end_date, type: typeof(DateTimeGraphType)).Description("End date of the plan");

            // Trường liên kết đến Plan
            Field<PlanType>(
                "plan",
                resolve: context => context.Source.Plan,
                description: "The associated plan object"
            );

        }
    }
}
