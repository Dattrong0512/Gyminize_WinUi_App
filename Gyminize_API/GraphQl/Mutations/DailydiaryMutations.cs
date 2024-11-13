using GraphQL;
using GraphQL.Types;
using Gyminize_API.Data.Model;
using Gyminize_API.Data.Models;
using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl.Type;
using Gyminize_API.GraphQl.Types;

namespace Gyminize_API.GraphQl.Mutations;

public class DailydiaryMutations : ObjectGraphType
{
    public DailydiaryMutations(DailydiaryRepository dailydiaryRepository)
    {
        Field<DailydiaryType>(
            "addDailydiary",
            "Is used to add a new diary",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<DailydiaryInputType>> { Name = "Dailydiary" }
            ),
            resolve: context =>
            {
                // Lấy đối tượng Dailydiary từ argument
                var dailydiary = context.GetArgument<Dailydiary>("dailydiary");

                if (dailydiary != null)
                {
                    // Gọi repository để thêm Dailydiary mới
                    return dailydiaryRepository.addDailydiary(dailydiary);
                }
                return null;
            });
    }
}
