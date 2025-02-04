using Microsoft.EntityFrameworkCore;

namespace PizzaStore.DB
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
    }

    public class PizzaDB : DbContext
    {
        public PizzaDB(DbContextOptions<PizzaDB> options) : base(options) { }

        public DbSet<Pizza> Pizzas { get; set; }

        private static List<Pizza> _pizzas = new List<Pizza>()
        {
            new Pizza{ Id=1, Name="Montemagno, Pizza shaped like a great mountain", Image = "https://http.pizza/100.jpg"},
            new Pizza{ Id=2, Name="The Galloway, Pizza shaped like a submarine, silent but deadly", Image = "https://http.pizza/101.jpg"},
            new Pizza{ Id=3, Name="The Noring, Pizza shaped like a Viking helmet, where's the mead", Image = "https://http.pizza/102.jpg"}
        };

        public static Pizza? GetPizza(int id) => _pizzas.FirstOrDefault(p => p.Id == id);
        public static List<Pizza> GetPizzas() => _pizzas;
        public static void CreatePizza(Pizza pizza) => _pizzas.Add(pizza);
        public static void UpdatePizza(Pizza pizza)
        {
            var index = _pizzas.FindIndex(p => p.Id == pizza.Id);
            if (index != -1) _pizzas[index] = pizza;
        }
        public static void RemovePizza(int id)
        {
            var pizza = GetPizza(id);
            if (pizza != null) _pizzas.Remove(pizza);
        }
    }
}