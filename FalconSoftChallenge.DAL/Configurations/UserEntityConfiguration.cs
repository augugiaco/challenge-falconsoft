using FalconSoftChallenge.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FalconSoftChallenge.DAL.Configurations
{
    internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .HasMaxLength(100)
            .IsRequired();

            builder.Property(x => x.Password)
                .HasMaxLength(100)
            .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(15)
                .IsRequired();
        }
    }
}
