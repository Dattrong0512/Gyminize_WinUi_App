using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Types;
using GraphQL;
using GraphQL.Types;
using Gyminize_API.GraphQl.Type;
namespace Gyminize_API.GraphQl.Queries
{
    public class DailydiaryQuery : ObjectGraphType
    {
        public DailydiaryQuery(DailydiaryRepository repository) 
        {
            Field<ListGraphType<DailydiaryType>>(
                "dailydiaries",
                resolve: context => repository.GetAllDailydiary()
            );
            Field<DailydiaryType>(
                "dailydiary",
                description: "Return a dailydiary by id",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>>
                    {
                        Name = "customer_id"
                    }
                ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("customer_id");
                    return repository.GetDailydiaryByIdCustomer(id);
                }
                );
        }
        
    }
}


