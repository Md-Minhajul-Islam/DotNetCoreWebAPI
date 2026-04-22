using System.ComponentModel.DataAnnotations;
using LibraryManagementAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LibraryManagementAPI.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Member");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.FullName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.IsActive)
            .HasDefaultValue(true);

        
        
        // Indexes
        builder.HasIndex(m => m.Email)
            .IsUnique()
            .HasDatabaseName("IX_Members_Email");
        
    }
}