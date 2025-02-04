using PizzaStore.DB;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizzas.db";

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlite<PizzaDB>(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/pizzas/{id}", (int id, PizzaDB db) => db.Pizzas.Find(id));
app.MapGet("/pizzas", (PizzaDB db) => db.Pizzas.ToList());
app.MapPost("/pizzas", (Pizza pizza, PizzaDB db) => {
    db.Pizzas.Add(pizza);
    db.SaveChanges();
    return Results.Created($"/pizzas/{pizza.Id}", pizza);
});
app.MapPut("/pizzas", (Pizza pizza, PizzaDB db) => {
    db.Pizzas.Update(pizza);
    db.SaveChanges();
    return Results.NoContent();
});
app.MapDelete("/pizzas/{id}", (int id, PizzaDB db) => {
    var pizza = db.Pizzas.Find(id);
    if (pizza != null)
    {
        db.Pizzas.Remove(pizza);
        db.SaveChanges();
    }
    return Results.NoContent();
});

app.Run();