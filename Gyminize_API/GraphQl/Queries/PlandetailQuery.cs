using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Types;
using GraphQL;
using GraphQL.Types;
using Gyminize_API.GraphQl.Type;

namespace Gyminize_API.GraphQl.Queries
{
    public class PlandetailQuery : ObjectGraphType
    {
        public PlandetailQuery(PlandetailRepository repository)
        {
            Field<PlandetailType>(
                "GetplandetailById",
                description: "Return a plandetail by id",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "customer_id"
                    }
                ),
                resolve: context =>
                {
                    int day = 1;
                    var id = context.GetArgument<int>("customer_id");
                    return repository.GetPlandetailById(id, day);
                }
                );
        }

        
    }
}


