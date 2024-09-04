using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.HasKey(e => new { e.Id });

            builder.Property(e => e.Email)
               .HasColumnType("varchar")
               .HasMaxLength(100)
               .IsRequired();

            builder.Property(e => e.FirstName)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(e => e.LastName)
               .HasColumnType("varchar")
               .HasMaxLength(50);

            builder.Property(e => e.Username)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired();

            builder.Property(e => e.Password)
               .HasColumnType("varchar")
               .HasMaxLength(250)
               .IsRequired();

            builder.Property<Guid>(e => e.CompanyId)
               .HasColumnType("uniqueidentifier")
               .IsRequired();

            builder.Property(e => e.Role)
               .HasColumnType("varchar")
               .HasMaxLength(50)
               .IsRequired();
        }
    }
}
