using FalconSoftChallenge.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FalconSoftChallenge.DAL.Configurations
{
    internal class ProductsPerOrderEntityConfiguration : IEntityTypeConfiguration<ProductsPerOrder>
    {
        public void Configure(EntityTypeBuilder<ProductsPerOrder> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.ProductId });
        }
    }
}
