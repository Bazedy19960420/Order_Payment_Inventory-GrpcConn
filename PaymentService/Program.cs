using Microsoft.EntityFrameworkCore;
using OrderService.Protos;
using PaymentService.Data;
using PaymentService.Models;
using PaymentService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt=>{
    opt.UseInMemoryDatabase("UsersDB");
});
builder.Services.AddGrpc();
builder.Services.AddLogging();
builder.Services.AddGrpcReflection();
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
app.MapGrpcService<PaymentOrderService>();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedDatabase(context);
}

app.Run();

void SeedDatabase(AppDbContext context)
{
    context.Database.EnsureCreated();

    if (!context.users.Any())
    {
        context.users.AddRange(
            new User { UserId = 1, Balance = 8500 },
            new User { UserId = 2, Balance = 4500 },
            new User { UserId = 3, Balance = 6000 }
        );

        context.SaveChanges();
    }
}
