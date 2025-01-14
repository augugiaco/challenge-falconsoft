using FalconSoftChallenge.Entities;

namespace FalconSoftChallenge.Business.DTO
{
    public record OrderDTO(Guid Id, string Description, DateTime CreatedDate, decimal Amount, OrderStatus Status, IReadOnlyCollection<ProductDTO> Products);

    public static class OrderDTOExtensions 
    {
        public static OrderDTO ToDTO(this Order order) 
        {
            var productsDTO = order
                .Products
                .Select(x => x.ToDTO())
                .ToList();

            return new OrderDTO(order.Id, order.Description, order.CreatedDate, order.Amount, order.Status, productsDTO);
        }
    }
}
