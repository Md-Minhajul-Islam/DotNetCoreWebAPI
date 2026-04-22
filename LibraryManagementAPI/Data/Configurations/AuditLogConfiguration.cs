using LibraryManagementAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementAPI.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLog");

        builder.HasKey(al => al.Id);

        builder.Property(al => al.EntityName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(al => al.Action)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(al => al.OccurredAt)
            .HasDefaultValueSql("GETUTCDATE()");
    }    
}
