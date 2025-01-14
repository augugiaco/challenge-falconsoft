namespace FalconSoftChallenge.Entities
{
    public class Product
    {
        public Guid Id { get; init; }

        public string Name { get; init; }

        public decimal Price { get; private set; }

        public virtual IEnumerable<ProductsPerOrder> LinkedOrders { get; set; }

        //Empty constructor added for Entity Framework purposes.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Product()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {

        }

        public Product(string name, decimal price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            LinkedOrders = new List<ProductsPerOrder>();
        }

        public void SetPrice(decimal price) => Price = price;
    }
}
