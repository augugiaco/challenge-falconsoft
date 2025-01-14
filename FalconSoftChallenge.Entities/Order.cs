namespace FalconSoftChallenge.Entities
{
    public class Order
    {
        public Guid Id { get; init; }

        public string Description { get; init; }

        public virtual User User { get; init; }

        public List<ProductsPerOrder> Products { get; private set; }

        public DateTime CreatedDate { get; init; }

        public OrderStatus Status { get; private set; }

        public decimal Amount { get; private set; }

        //Empty constructor added for Entity Framework purposes.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Order()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {

        }

        public Order(User user, string description)
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
            Status = OrderStatus.Created;
            Description = description;
            User = user;
            Products = new List<ProductsPerOrder>();
        }

        public void AddProducts(IEnumerable<ProductsPerOrder> products) 
        {
            if(Status == OrderStatus.Created) 
            {
                Products.AddRange(products);

                RefreshAmount();

                return;
            }

            throw new Exception("Order must be in Created status to allow changes");
        }

        public void RemoveProducts(IEnumerable<Guid> productsToRemoveIds)
        {
            if (Status == OrderStatus.Created)
            {         
                Products.RemoveAll(x => productsToRemoveIds.Contains(x.ProductId));

                RefreshAmount();

                return;
            }

            throw new Exception("Order must be in Created status to allow changes");
        }

        public void SetStatus(OrderStatus status) => Status = status;

        public void RefreshAmount() 
        {
            if(Status != OrderStatus.Created) throw new Exception("Order must be in Created status to allow changes");

            Amount = Products.Sum(x => x.Quantity * x.Product.Price);
        }
    }

    public enum OrderStatus
    {
        Created = 0,
        Processed = 1,
        Cancelled = 2
    }
}
