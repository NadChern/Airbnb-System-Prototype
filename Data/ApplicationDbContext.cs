using AirbnbREST.Models;
using Microsoft.EntityFrameworkCore;

// Need to check with Kamie! Its our ER diagram implementation 
// Adjusting Entity Framework Core setup to map models to PostgreSQL tables
// (Database-First approach)
namespace AirbnbREST.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Availability> Availabilities { get; set; }
    public DbSet<PropertyPhoto> PropertyPhotos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Property>().ToTable("properties");
        modelBuilder.Entity<Booking>().ToTable("bookings");
        modelBuilder.Entity<Availability>().ToTable("availabilities");
        modelBuilder.Entity<PropertyPhoto>().ToTable("property_photos");
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id"); // Fixes `Id` issue
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.MiddleName).HasColumnName("middle_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Email).HasColumnName("email").IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
            entity.Property(e => e.ProfilePicLink).HasColumnName("profile_pic_link");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.Role).HasColumnName("role").HasConversion<string>().IsRequired();
        
            // Ensure unique emails
            entity.HasIndex(e => e.Email).IsUnique();
        });
        
        // Configure relationships
        // User -> Property (One-to-Many)
        modelBuilder.Entity<Property>()
            .HasOne<User>()
            .WithMany() // user can own multiple properties
            .HasForeignKey(p => p.Owner)
            .OnDelete(DeleteBehavior.Cascade); // if owner is deleted, property also should be removed

        // Property -> Booking (One-to-Many)
        modelBuilder.Entity<Booking>()
            .HasOne<Property>()
            .WithMany() // property can have multiple bookings
            .HasForeignKey(b => b.PropertyId)
            .OnDelete(DeleteBehavior.Restrict); // If a property is deleted, bookings should saved! CHECK

        // User -> Booking (One-to-Many)
        modelBuilder.Entity<Booking>()
            .HasOne<User>()
            .WithMany() // user can have multiple bookings
            .HasForeignKey(b => b.GuestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Property -> Availability (One-to-Many)
        modelBuilder.Entity<Availability>()
            .HasOne<Property>()
            .WithMany() // property can have multiple availabilities
            .HasForeignKey(a => a.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure photos configuration
        modelBuilder.Entity<PropertyPhoto>()
            .HasOne<Property>()
            .WithMany(p => p.Photos)
            .HasForeignKey(pp => pp.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Unique Constraints
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique(); // Ensure unique emails
    }
}