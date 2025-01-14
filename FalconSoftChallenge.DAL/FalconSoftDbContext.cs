using FalconSoftChallenge.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FalconSoftChallenge.DAL
{
    public class FalconSoftDbContext : DbContext
    {
        public FalconSoftDbContext(){}

        public FalconSoftDbContext(DbContextOptions<FalconSoftDbContext> opts) : base(opts)
        {
            Database.Migrate();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            var seedData = BuildSeedData();

            optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                if (await context.Set<Order>().AnyAsync()) return;

                await context.Set<User>().AddRangeAsync(seedData.Users);
                await context.Set<Product>().AddRangeAsync(seedData.Products);
                await context.Set<Order>().AddRangeAsync(seedData.Orders);

                await context.SaveChangesAsync(cancellationToken);
            })
            .UseSeeding((context, _) =>
            {
                if (context.Set<Order>().Any()) return;

                context.Set<User>().AddRange(seedData.Users);
                context.Set<Product>().AddRange(seedData.Products);
                context.Set<Order>().AddRange(seedData.Orders);

                context.SaveChanges();
            });
        }

        private (IEnumerable<Order> Orders, IEnumerable<Product> Products, IEnumerable<User> Users) BuildSeedData() 
        {
            var user1 = new User("Augusto", "augusto@server.com", "racingcampeon2024");
            var user2 = new User("Federico", "federico@server.com", "123456");

            var product1 = new Product("Pelota", 10m);
            var product2 = new Product("Bicicleta", 5000m);

            var orders = new List<Order>();
            var randomQuantity = new Random();

            for (int i = 0; i < 20; i++)
            {
                var quantity = randomQuantity.Next(1, 5);
                var order = new Order(user1, $"Compra Nº{i}");
                var products = new List<ProductsPerOrder>
                {
                    new ProductsPerOrder(order, product1, quantity),
                    new ProductsPerOrder(order, product2, quantity)
                };

                order.AddProducts(products);

                orders.Add(order);
            }

            return (orders, 
                new List<Product> { product1, product2, product2 }, 
                new List<User> { user1, user2 });    
        }
    }
}
