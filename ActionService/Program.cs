using ActionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var name = "DefaultConnection";
var connection = builder.Configuration.GetConnectionString(name);
builder.Services.AddDbContext<AuctionDbContext>(opt =>
{
    opt.UseNpgsql(connection);
});


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
