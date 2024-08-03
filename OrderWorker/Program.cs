using Microsoft.EntityFrameworkCore;
using OrderWorker.Models;
using OrderWorker.Services;
using PaymentService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Configure(builder.Configuration.GetSection("Kestrel"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseInMemoryDatabase("OrderDb"));
builder.Services.AddScoped<OrderServices>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

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
            new Item { Id =1,Price = 30 },
            new Item { Id =2,Price = 50},
            new Item { Id =3,Price = 60 },
            new Item { Id =4,Price = 70}
 

        );

        context.SaveChanges();
    }
}
