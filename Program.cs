using TemperatureAlertSystem.Queries;
using TemperatureAlertSystem.Mutators;
using TemperatureAlertSystem.ThermometerLogic;

/* Author: David DLV
* Date:5/18/2023
* This class is the main class for the application, it sets up the GraphQL API in an up to date way, in the style for .NET 6.
*/
var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddGraphQLServer()
    .AddQueryType<CurrentTemperatureQuery>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .AddMutationType<Mutation>()
    .AddInMemorySubscriptions();

ThermometerAlertSystem.Start();

var app = builder.Build();
app.Urls.Add("http://localhost:7412");
app.MapGraphQL();


app.Run();

