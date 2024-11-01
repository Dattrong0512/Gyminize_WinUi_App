using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Types;
using GraphQL;
using GraphQL.Types;

namespace Gyminize_API.GraphQl.Queries
{
    public class FoodQueries : ObjectGraphType
    {
        public FoodQueries(FoodRepository repository)
        {
            Field<ListGraphType<FoodType>>(
                "foods",
                description: "Return all foods",
                resolve: context => repository.GetAllFood()
            );
            Field<FoodType>(
                "food",
                description: "Return a food by id",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "food_id"
                    }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("food_id");
                    return repository.GetFoodById(id);
                }
                );
        }

    }
}


