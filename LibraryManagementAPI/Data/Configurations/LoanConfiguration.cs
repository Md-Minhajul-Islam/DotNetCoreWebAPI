using LibraryManagementAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementAPI.Data.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loan");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.BookId)
            .IsRequired();
        
        builder.Property(l => l.MemberId)
            .IsRequired();
        
        builder.Property(l => l.BorrowedAt)
            .HasDefaultValueSql("GETUTCDATE()");
        
        builder.Property(l => l.DueDate)
            .IsRequired();

        
        // Relations
        builder.HasOne(l => l.Book)
            .WithMany(b => b.Loans)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Member)
            .WithMany(m => m.Loans)
            .HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        
    }
}