using LibraryManagementAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementAPI.Data.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Book");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(b => b.ISBN)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(b => b.AvailableCopies)
            .IsRequired();

        
        //  Indexes
        builder.HasIndex(b => b.ISBN)
            .IsUnique()
            .HasDatabaseName("IX_Books_ISBN");


        // Relations
    }
}