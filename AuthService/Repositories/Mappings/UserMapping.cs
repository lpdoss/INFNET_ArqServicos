using AuthService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Repositories.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().HasColumnName("Id");
        builder.Property(x => x.Name).IsRequired().HasColumnName("Name");
        builder.Property(x => x.Login).IsRequired().HasColumnName("Login");
        builder.Property(x => x.Email).IsRequired().HasColumnName("Email");
        builder.Property(x => x.Password).IsRequired().HasColumnName("Password");
    }
}