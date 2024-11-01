using Gyminize_API.Data;
using Gyminize_API.Data.Repositories;
using Gyminize_API.GraphQl;
using Gyminize_API.GraphQl.Queries;
using GraphQL.Server;
using Microsoft.EntityFrameworkCore;
using Gyminize_API.GraphQL;
using GraphQL.Server.Transports.AspNetCore.SystemTextJson;
using Gyminize_API.GraphQl.Mutations;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<CustomerQuery>();
builder.Services.AddScoped<CustomerMutation>();
builder.Services.AddScoped<CustomerHealthRepository>();
builder.Services.AddScoped<CustomerhealthMutations>();
builder.Services.AddScoped<CustomerhealthQuery>();
builder.Services.AddScoped<DailydiaryRepository>();
builder.Services.AddScoped<DailydiaryQuery>();
builder.Services.AddScoped<FooddetailRepository>();
builder.Services.AddScoped<FooddetailQuery>();
builder.Services.AddScoped<FoodRepository>();
builder.Services.AddScoped<FoodQueries>();
builder.Services.AddScoped<AppSchema>();

// register graphQL
builder.Services.AddGraphQL().AddSystemTextJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EntityDatabaseContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("SqlDbCon"),
        npgsqlOptions =>
        {
            npgsqlOptions.CommandTimeout(60);  // Thiết lập thời gian chờ 60 giây cho mỗi truy vấn
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,                 // Số lần thử lại tối đa
                maxRetryDelay: TimeSpan.FromSeconds(5),  // Thời gian chờ giữa mỗi lần thử lại
                 errorCodesToAdd: null
            );
        }),
    ServiceLifetime.Scoped
);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=>c.SwaggerEndpoint("/swagger/v1/swagger.json","Gyminize_API"));
}


app.UseHttpsRedirection();
app.UseGraphQL<AppSchema>();
app.UseGraphQLGraphiQL("/ui/graphql");
app.UseAuthorization();

app.MapControllers();

app.Run();
