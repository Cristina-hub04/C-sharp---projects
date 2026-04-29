using Bookstore.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Map the endpoints from BookEndpoints
app.MapBookEndpoints();

app.Run();