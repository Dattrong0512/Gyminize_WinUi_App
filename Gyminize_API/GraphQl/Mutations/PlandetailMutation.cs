using GraphQL;
using GraphQL.Types;
using Gyminize_API.Data.Model;
using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Types;


namespace Gyminize_API.GraphQl.Mutations
{
    public class PlandetailMutation : ObjectGraphType
    {
        public PlandetailMutation(PlandetailRepository plandetailRepository)
        {
            Field<PlandetailType>
                (
                "addPlandetail",
                "IsUsed to add a new plandetail",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "customer_id"
                    },
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "plan_id"
                    }
                    ),
                resolve: context =>
                {
                    var customer_id = context.GetArgument<int>("customer_id");
                    var plan_id = context.GetArgument<int>("plan_id");
                    if (customer_id != null && plan_id != null)
                    {
                        return plandetailRepository.AddPlandetail(customer_id, plan_id);
                    }
                    return null;
                }
                );

        }
    }
}



