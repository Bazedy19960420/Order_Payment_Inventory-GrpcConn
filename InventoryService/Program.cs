using InventoryService.Data;
using InventoryService.Models;
using InventoryService.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(o=>o.UseInMemoryDatabase("InventoryDb"));
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddLogging();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGrpcReflectionService();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<OrderInventoryServices>();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedDatabase(context);
}

app.Run();

void SeedDatabase(AppDbContext context)
{
    context.Database.EnsureCreated();

    if (!context.Items.Any())
    {
        context.Items.AddRange(
            new Item { Id = 1,Name = "Rice",Quantity =220  },
            new Item { Id = 2,Name = "Milk",Quantity = 350 },
            new Item { Id = 3,Name = "Chocolate",Quantity = 12  },
            new Item { Id = 4,Name = "Butter",Quantity = 1   }
 

        );

        context.SaveChanges();
    }
}
