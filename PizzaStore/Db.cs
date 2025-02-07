using Microsoft.EntityFrameworkCore;

namespace PizzaStore.DB
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
    }

     public class CartItem
    {
        public int Id { get; set; }
        public int PizzaId { get; set; }
        public int Quantity { get; set; }

        // Navigation property
        public Pizza Pizza { get; set; } = null!;
    }

    public class PizzaDB : DbContext
    {
        public PizzaDB(DbContextOptions<PizzaDB> options) : base(options) { }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data
            modelBuilder.Entity<Pizza>().HasData(
                new Pizza { Id = 1, Name = "Montemagno, Pizza shaped like a great mountain", Image = "https://http.pizza/100.jpg" },
                new Pizza { Id = 2, Name = "The Galloway, Pizza shaped like a submarine, silent but deadly", Image = "https://http.pizza/101.jpg" },
                new Pizza { Id = 3, Name = "The Noring, Pizza shaped like a Viking helmet, where's the mead", Image = "https://http.pizza/102.jpg" }
            );
        }
    }


}