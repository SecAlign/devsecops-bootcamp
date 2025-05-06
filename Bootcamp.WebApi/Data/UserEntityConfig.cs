// // Created On: 2025.05.06
// // Create by: althunibat

using Bootcamp.WebApi.Api;
using Bootcamp.WebApi.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bootcamp.WebApi.Data;

public class UserEntityConfig
    : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        builder.HasKey(ld => ld.Id);
        builder.Property(ld => ld.Id).ValueGeneratedOnAdd().UseIdentityColumn();
        builder.Property(ld => ld.FirstName).IsRequired().HasMaxLength(128);
        builder.Property(ld => ld.LastName).IsRequired().HasMaxLength(128);
        builder.Property(ld => ld.Email).IsRequired().HasMaxLength(128);
        builder.Property(ld => ld.RowVersion).IsRowVersion();
        builder.HasIndex(ld => new { ld.FirstName, ld.LastName, ld.Email }).HasMethod("GIN").IsTsVectorExpressionIndex("english");
    }
}