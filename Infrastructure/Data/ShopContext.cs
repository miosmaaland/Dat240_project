using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.SharedKernel;
using SmaHauJenHoaVij.Core.Domain.Users;
using SmaHauJenHoaVij.Core.Domain.Users.Couriers;
using SmaHauJenHoaVij.Core.Domain.Users.Customers;
using SmaHauJenHoaVij.Core.Domain.Users.Admins;
using SmaHauJenHoaVij.Core.Domain.Products;
using SmaHauJenHoaVij.Core.Domain.Cart;
using SmaHauJenHoaVij.Core.Domain.Ordering;
using System.Text;
using System.Security.Cryptography;
using SmaHauJenHoaVij.Core.Domain.Fulfillment;
using SmaHauJenHoaVij.Core.Domain.Invoicing;

namespace SmaHauJenHoaVij.Infrastructure.Data;

public class ShopContext : DbContext
{
    private readonly IMediator _mediator;

    public ShopContext(DbContextOptions configuration, IMediator mediator) : base(configuration)
    {
        _mediator = mediator;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<FoodItem> FoodItems { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<Reimbursement> Reimbursements { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasDiscriminator<string>("UserType")
            .HasValue<Customer>("Customer")
            .HasValue<Courier>("Courier")
            .HasValue<Admin>("Administrator");

        modelBuilder.Entity<FoodItem>(entity =>
        {
            entity.HasKey(fi => fi.Id);

            // Configure Picture as a value object
            entity.OwnsOne(fi => fi.Picture, picture =>
            {
                picture.Property(p => p.Path)
                       .HasColumnName("PicturePath") // Store as a single column in FoodItem table
                       .IsRequired();
            });
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasMany(c => c.Items)
                  .WithOne()
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(i => i.Id);
        });

        // Order Configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.OrderDate).IsRequired();
            entity.Property(o => o.Status).IsRequired();
            entity.Property(o => o.Notes);

            // Configure Location as a value object
            entity.OwnsOne(o => o.Location, location =>
            {
                location.Property(l => l.Building)
                        .HasColumnName("Building")
                        .IsRequired();

                location.Property(l => l.RoomNumber)
                        .HasColumnName("RoomNumber")
                        .IsRequired();

                location.Property(l => l.Notes)
                        .HasColumnName("LocationNotes");
            });

            entity.OwnsOne(o => o.DeliveryFee, fee =>
            {
                fee.Property(f => f.Value)
                   .HasColumnName("DeliveryFee")
                   .IsRequired();
            });

            entity.Property(o => o.CustomerId).IsRequired();
            entity.Property(o => o.CustomerName).IsRequired();

            // Configure relationship with OrderLines
            entity.HasMany(o => o.OrderLines)
                  .WithOne()
                  .HasForeignKey("OrderId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.HasKey(ol => ol.Id);
            entity.Property(ol => ol.ItemName).IsRequired();
            entity.Property(ol => ol.Price).IsRequired();
            entity.Property(ol => ol.Amount).IsRequired();
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.OrderId).IsRequired();
            entity.Property(o => o.CourierId);
            entity.Property(o => o.Tip);


            // Link to Reimbursement as a foreign key
            entity.HasOne<Reimbursement>()
                .WithOne()
                .HasForeignKey<Offer>(o => o.Reimbursement)
                .OnDelete(DeleteBehavior.Cascade); // Deleting an Offer deletes its Reimbursement
        });

        modelBuilder.Entity<Reimbursement>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Amount).IsRequired();
            entity.Property(r => r.CourierId);
            entity.Property(r => r.InvoiceId);
            entity.Property(r => r.Tip);
        });


        // Seed Admin Users
        var adminId = Guid.NewGuid();
        modelBuilder.Entity<Admin>().HasData(new Admin
        {
            Id = adminId,
            Name = "Admin",
            Email = "admin@example.com",
            PasswordHash = CreatePasswordHash("Admin123"), // Replace with a securely hashed password
            PasswordNeedsReset = true, //Admin must reset the password on first login
            PasswordResetToken = null
        });
        var adminId2 = Guid.NewGuid();
        modelBuilder.Entity<Admin>().HasData(new Admin
        {
            Id = adminId2,
            Name = "Admin2",
            Email = "admin2@example.com",
            PasswordHash = CreatePasswordHash("Admin2123"), // Replace with a securely hashed password
            PasswordNeedsReset = true, // Admin must reset the password on first login
            PasswordResetToken = null
            
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Amount).IsRequired();
            entity.Property(i => i.Status).IsRequired();
            entity.Property(i => i.Tip);
        });  

        base.OnModelCreating(modelBuilder);
    }

    private string CreatePasswordHash(string password)
    {
        using var sha256 = SHA256.Create();
        return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // Ignore events if no dispatcher provided
        if (_mediator == null) return result;

        // Dispatch events only if save was successful
        var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.Events.Any())
            .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.Events.ToArray();
            entity.Events.Clear();
            foreach (var domainEvent in events)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }
        return result;
    }

    public override int SaveChanges() => SaveChangesAsync().GetAwaiter().GetResult();
}
