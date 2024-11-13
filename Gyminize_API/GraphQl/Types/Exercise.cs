using Gyminize_API.Data.Model;
using GraphQL.Types;

namespace Gyminize_API.GraphQl.Types
{
    public class ExerciseType : ObjectGraphType<Exercise>
    {
        public ExerciseType()
        {
            Field(x => x.exercise_id, type: typeof(IntGraphType)).Description("Id of the exercise");
            Field(x => x.exercise_name).Description("Name of the exercise");
            Field(x => x.description).Description("Description of the exercise");
            Field(x => x.linkvideo).Description("Link to the exercise video");
            Field(x => x.reps, type: typeof(IntGraphType)).Description("Number of repetitions");

        }
    }
}
