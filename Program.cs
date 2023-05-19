using TemperatureAlertSystem.Queries;
using TemperatureAlertSystem.Mutators;

var builder = WebApplication.CreateBuilder(args);
builder.Services   
    .AddGraphQLServer()
    .AddQueryType<CurrentTemperatureQuery>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddMutationType<Mutation>() 
    .AddInMemorySubscriptions();   

var app = builder.Build();

app.UseWebSockets();
app.MapGraphQL();

app.Run();
