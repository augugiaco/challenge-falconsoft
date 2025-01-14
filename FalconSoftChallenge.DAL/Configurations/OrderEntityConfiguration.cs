using FalconSoftChallenge.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FalconSoftChallenge.DAL.Configurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Amount)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .HasMany(x => x.Products)
                .WithOne(x => x.Order);
        }
    }
}
