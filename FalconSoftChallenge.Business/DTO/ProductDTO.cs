using FalconSoftChallenge.Entities;

namespace FalconSoftChallenge.Business.DTO
{
    public record ProductDTO(Guid Id, string Name, decimal Price, int Quantity);

    public static class ProductDTOExtensions
    {
        public static ProductDTO ToDTO(this ProductsPerOrder productPerOrder)
            => new ProductDTO(productPerOrder.ProductId, productPerOrder.Product.Name, productPerOrder.Product.Price, productPerOrder.Quantity);
    }
}
