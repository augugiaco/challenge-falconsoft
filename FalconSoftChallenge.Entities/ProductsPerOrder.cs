namespace FalconSoftChallenge.Entities
{
    public class ProductsPerOrder
    {
        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }

        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int Quantity { get; private set; }

        //Empty constructor added for EF purposses.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public ProductsPerOrder()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
                
        }

        public ProductsPerOrder(Order order, Product product, int quantity)
        {
            OrderId = order.Id;
            Order = order;

            ProductId = product.Id;
            Product = product;

            Quantity = quantity;
        }

        public void SetQuantity(int quantity) 
        {
            if (Order?.Status != OrderStatus.Created) 
                throw new InvalidOperationException();

            Quantity = quantity;
        }
    }
}
