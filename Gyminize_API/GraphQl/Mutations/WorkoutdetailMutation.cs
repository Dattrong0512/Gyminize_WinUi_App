using GraphQL;
using GraphQL.Types;
using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Types;
using Microsoft.Identity.Client;

namespace Gyminize_API.GraphQl.Mutations
{
    public class WorkoutdetailMutation : ObjectGraphType
    {
        public WorkoutdetailMutation(WorkoutdetailRepository workoutdetailRepository) 
        {
            Field<WorkoutdetailType>
            (
                "updateDecriptionsWorkoutdetail",
                "IsUsed to update descriptions of a workoutdetail if customer finish",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "workoutdetail_id"
                    },
                    new QueryArgument<NonNullGraphType<StringGraphType>>
                    {
                        Name = "descriptions"
                    }
                 ),
                resolve: context =>
                {
                    var workoutdetail_id = context.GetArgument<int>("workoutdetail_id");
                    var descriptions = context.GetArgument<string>("descriptions");
                    if (workoutdetail_id != null && descriptions != null)
                    {
                        return workoutdetailRepository.UpdateDecription(workoutdetail_id, descriptions);
                    }
                    return null;
                }

            );
        }
    }
}


