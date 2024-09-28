var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var name = "ReverseProxy";
var proxy = builder.Configuration.GetSection(name);
builder.Services.AddReverseProxy().LoadFromConfig(proxy);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapReverseProxy();

app.Run();
