using AirbnbREST.Models;
using Microsoft.EntityFrameworkCore;
namespace AirbnbREST.Data;

// Represents the database context for the AirbnbREST application.
// This class is responsible for interacting with PostgreSQL database
// using Entity Framework Core.
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

    // For DateTime UTC handling
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        // Enable legacy timestamp behavior to handle DateTime conversions
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    // Configures the database schema and relationships 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // Map entity classes to PostgreSQL table names
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Property>().ToTable("properties");
        modelBuilder.Entity<PropertyPhoto>().ToTable("property_photos");
        modelBuilder.Entity<Booking>().ToTable("bookings");
        modelBuilder.Entity<Availability>().ToTable("availabilities");

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");
            entity.Property(e => e.FirstName).HasColumnName("first_name").IsRequired();
            entity.Property(e => e.MiddleName).HasColumnName("middle_name");
            entity.Property(e => e.LastName).HasColumnName("last_name").IsRequired();
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Email).HasColumnName("email").IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
            entity.Property(e => e.ProfilePicLink).HasColumnName("profile_pic_link");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.Role).HasColumnName("role")
                .HasConversion<string>()
                .IsRequired();

            // Ensure unique emails
            entity.HasIndex(e => e.Email).IsUnique();
        });


        // Configure Property entity
        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Owner).HasColumnName("owner").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.Bedrooms).HasColumnName("bedrooms").IsRequired();
            entity.Property(e => e.Bathrooms).HasColumnName("bathrooms").IsRequired();
            entity.Property(e => e.SquareFeet).HasColumnName("square_feet").IsRequired();
            entity.Property(e => e.PricePerNight).HasColumnName("price_per_night").IsRequired();
            entity.Property(e => e.Title).HasColumnName("title").IsRequired();
            entity.Property(e => e.About).HasColumnName("about");
            entity.Property(e => e.StreetAddress).HasColumnName("street_address").IsRequired();
            entity.Property(e => e.City).HasColumnName("city").IsRequired();
            entity.Property(e => e.State).HasColumnName("state").IsRequired();
            entity.Property(e => e.ZipCode).HasColumnName("zip_code").IsRequired();

            // Configure relationship with User (Owner)
            entity.HasOne<User>()
                .WithMany() // user can own multiple properties
                .HasForeignKey(p => p.Owner)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(); // ensures relationship itself is required
        });


        // PropertyPhoto configuration
        modelBuilder.Entity<PropertyPhoto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PropertyId).HasColumnName("property_id").IsRequired();
            entity.Property(e => e.PhotoUrl).HasColumnName("photo_link").IsRequired();

            // Configure relationship with Property
            entity.HasOne<Property>()
                .WithMany(p => p.Photos)
                .HasForeignKey(pp => pp.PropertyId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });


        // Configure Booking entity
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PropertyId).HasColumnName("property_id").IsRequired();
            entity.Property(e => e.GuestId).HasColumnName("guest_id").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("NOW()");
            entity.Property(e => e.StartDate).HasColumnName("start_date").IsRequired();
            entity.Property(e => e.EndDate).HasColumnName("end_date").IsRequired();
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();

            // Configure relationship with Property
            entity.HasOne<Property>()
                .WithMany() // property can have multiple bookings
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Configure relationship with User (Guest)
            entity.HasOne<User>()
                .WithMany() // user can have multiple bookings
                .HasForeignKey(b => b.GuestId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });


        // Availability configuration
        modelBuilder.Entity<Availability>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PropertyId).HasColumnName("property_id").IsRequired();
            entity.Property(e => e.StartDate).HasColumnName("start_date").IsRequired();
            entity.Property(e => e.EndDate).HasColumnName("end_date").IsRequired();

            // Configure relationship with Property
            entity.HasOne<Property>()
                .WithMany() // property can have multiple availabilities
                .HasForeignKey(a => a.PropertyId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(); // Ensure relationship is required
        });
    }
}