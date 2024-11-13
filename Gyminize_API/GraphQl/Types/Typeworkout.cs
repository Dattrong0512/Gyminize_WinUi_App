using Gyminize_API.Data.Model;
using GraphQL.Types;

namespace Gyminize_API.GraphQl.Types
{
    public class TypeworkoutType : ObjectGraphType<Typeworkout>
    {
        public TypeworkoutType()
        {
            Field(x => x.typeworkout_id, type: typeof(IntGraphType)).Description("Id of the type workout");
            Field(x => x.workoutday_type).Description("Type of workout day (e.g., push, pull, leg)");
            Field(x => x.description).Description("Description of the type workout");

            // Thêm trường liên kết đến Exercisedetails
            Field<ListGraphType<ExercisedetailType>>(
                "exercisedetails",
                resolve: context => context.Source.Exercisedetails,
                description: "List of exercise details associated with the type workout"
            );

        }
    }
}
