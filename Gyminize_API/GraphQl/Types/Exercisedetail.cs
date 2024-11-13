using Gyminize_API.Data.Model;
using GraphQL.Types;

namespace Gyminize_API.GraphQl.Types
{
    public class ExercisedetailType : ObjectGraphType<Exercisedetail>
    {
        public ExercisedetailType()
        {
            Field(x => x.exercisedetail_id, type: typeof(IntGraphType)).Description("Id of the exercise detail");
            Field(x => x.typeworkout_id, type: typeof(IntGraphType)).Description("Id of the type workout");
            Field(x => x.exercise_id, type: typeof(IntGraphType)).Description("Id of the exercise");
            Field(x => x.workout_sets, type: typeof(IntGraphType)).Description("Number of workout sets");

            // Thêm trường liên kết đến Exercise
            Field<ExerciseType>(
                "exercise",
                resolve: context => context.Source.Exercise,
                description: "The associated exercise object"
            );

        }
    }
}
