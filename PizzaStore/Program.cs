using PizzaStore.DB;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizzas.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSqlite<PizzaDB>(connectionString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Welcome to PizzaStore API!");

// ** Pizza Endpoints **
app.MapGet("/pizzas", (PizzaDB db) => db.Pizzas.ToList());

app.MapDelete("/pizzas/{id}", (int id, PizzaDB db) =>
{
    var pizza = db.Pizzas.Find(id);
    if (pizza == null)
    {
        return Results.NotFound("Pizza not found.");
    }

    db.Pizzas.Remove(pizza);
    db.SaveChanges();
    return Results.NoContent();
});

app.MapGet("/pizzas/{id}", (int id, PizzaDB db) =>
{
    var pizza = db.Pizzas.Find(id);
    return pizza != null ? Results.Ok(pizza) : Results.NotFound("Pizza not found.");
});

// ** Cart Endpoints **
app.MapGet("/cart", (PizzaDB db) =>
{
    var cartItems = db.CartItems.Include(ci => ci.Pizza).ToList();
    return Results.Ok(cartItems);
});

app.MapPost("/cart", (CartItem cartItem, PizzaDB db) =>
{
    var pizza = db.Pizzas.Find(cartItem.PizzaId);
    if (pizza == null)
    {
        return Results.BadRequest("Invalid PizzaId. Pizza does not exist.");
    }

    var existingCartItem = db.CartItems.FirstOrDefault(ci => ci.PizzaId == cartItem.PizzaId);
    if (existingCartItem != null)
    {
        existingCartItem.Quantity += cartItem.Quantity;
    }
    else
    {
        db.CartItems.Add(cartItem);
    }

    db.SaveChanges();
    return Results.Created($"/cart/{cartItem.Id}", cartItem);
});

app.MapPut("/cart/{id}", (int id, CartItem updatedCartItem, PizzaDB db) =>
{
    var cartItem = db.CartItems.Find(id);
    if (cartItem == null)
    {
        return Results.NotFound("Cart item not found.");
    }

    cartItem.Quantity = updatedCartItem.Quantity;
    db.SaveChanges();
    return Results.NoContent();
});

app.MapDelete("/cart/{id}", (int id, PizzaDB db) =>
{
    var cartItem = db.CartItems.Find(id);
    if (cartItem == null)
    {
        return Results.NotFound("Cart item not found.");
    }

    db.CartItems.Remove(cartItem);
    db.SaveChanges();
    return Results.NoContent();
});

app.Run();