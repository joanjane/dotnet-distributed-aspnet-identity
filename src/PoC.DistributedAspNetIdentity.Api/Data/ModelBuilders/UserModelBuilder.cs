using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoC.DistributedAspNetIdentity.Api.Domain;

namespace PoC.DistributedAspNetIdentity.Api.Data.ModelBuilders
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(255);
            
            builder.Property(p => p.Surname)
                .HasMaxLength(255);
        }
    }
}
