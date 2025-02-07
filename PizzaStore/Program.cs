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
app.MapPost("/cart", (CartItem cartItem, PizzaDB db) =>
{
    db.CartItems.Add(cartItem);
    db.SaveChanges();
    return Results.Created($"/cart/{cartItem.Id}", cartItem);
});

app.MapPut("/cart/{id}", (int id, CartItem updatedCartItem, PizzaDB db) =>
{
    var cartItem = db.CartItems.Find(id);
    if (cartItem != null)
    {
        cartItem.Quantity = updatedCartItem.Quantity;
        db.SaveChanges();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.MapDelete("/cart/{id}", (int id, PizzaDB db) =>
{
    var cartItem = db.CartItems.Find(id);
    if (cartItem != null)
    {
        db.CartItems.Remove(cartItem);
        db.SaveChanges();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.MapGet("/cart", (PizzaDB db) =>
{
    var cartItems = db.CartItems.Include(ci => ci.Pizza).ToList();
    return Results.Ok(cartItems);
});


app.Run();