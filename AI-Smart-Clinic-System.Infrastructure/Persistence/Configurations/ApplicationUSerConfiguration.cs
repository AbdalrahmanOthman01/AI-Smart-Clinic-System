using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AI_Smart_Clinic_System.Domain.Entities;

namespace AI_Smart_Clinic_System.Infrastructure.Persistence.Configurations
{
    internal class ApplicationUSerConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasMaxLength(20)
                .IsRequired();

            builder.OwnsMany(u => u.RefreshTokens);
        }
    }
}
