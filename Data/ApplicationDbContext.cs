using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Updated namespace for ASP.NET Core Identity
using Microsoft.EntityFrameworkCore;
using TraceLink.Models;

namespace TraceLink.Data
{
    public class ApplicationDbContext : IdentityDbContext<TraceLinkUser> // Use ASP.NET Core IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define DbSets for your models
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemCategoryParameter> ItemCategoryParameters { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemStock> ItemStocks { get; set; }
        public DbSet<ItemStockParameter> ItemStockParameters { get; set; }
        public DbSet<UnitMap> UnitMaps { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<TaxDetails> TaxDetails { get; set; }
        public DbSet<TaggingHistory> TaggingHistories { get; set; }
        public DbSet<TaggingRequest> TaggingRequests { get; set; }
        public DbSet<TaggingStatus> TaggingStatuses { get; set; }
        public DbSet<TagDetails> TagDetails { get; set; } 
        public DbSet<Document> Documents { get; set; }
        public DbSet<ChatMessage> ChatMessages {  get; set; }
        public DbSet<ItemParameter> ItemParameters { get; set; }
        
        public DbSet<Dealer> Dealers { get; set; }  
        
        public DbSet<SubDealer> SubDealers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for ItemCategory
            modelBuilder.Entity<ItemCategory>().HasData(
                new ItemCategory { ItemCategoryId = 1, Name = "AIS-140" },
                new ItemCategory { ItemCategoryId = 2, Name = "E-SIM Services" },
                new ItemCategory { ItemCategoryId = 3, Name = "E-SIM" },
                new ItemCategory { ItemCategoryId = 4, Name = "Accessories" }
            );

            // Seed data for TaggingStatus
            modelBuilder.Entity<TaggingStatus>().HasData(
                new TaggingStatus { Id = 1, Name = "Open" },
                new TaggingStatus { Id = 2, Name = "Pending" },
                new TaggingStatus { Id = 3, Name = "In Progress" },
                new TaggingStatus { Id = 4, Name = "Completed" }
            );

            // Seed data for Items
            modelBuilder.Entity<Item>().HasData(
                new Item
                {
                    Id = 1,
                    IMEI = "123456789012345",
                    Name = "VLTD Device A",
                    IsTagged = false,
                    Category = "VLTD Device",
                    TaggingDate = null
                },
                new Item
                {
                    Id = 2,
                    IMEI = "987654321098765",
                    Name = "Panic Button B",
                    IsTagged = true,
                    Category = "Panic Button",
                    TaggingDate = new DateTime(2025, 1, 15)
                },
                new Item
                {
                    Id = 3,
                    IMEI = "456789123456789",
                    Name = "VLTD Device C",
                    IsTagged = false,
                    Category = "VLTD Device",
                    TaggingDate = new DateTime(2025, 1, 10)
                }
            );
            modelBuilder.Entity<TaggingRequest>()
       .HasOne(tr => tr.Dealer)
       .WithMany()
       .HasForeignKey(tr => tr.DealerId)
       .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<TaggingRequest>()
                .HasOne(tr => tr.SubDealer)
                .WithMany()
                .HasForeignKey(tr => tr.SubDealerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<TaggingRequest>()
                .HasOne(tr => tr.TaggingStatus)
                .WithMany()
                .HasForeignKey(tr => tr.TaggingStatusId)
                .OnDelete(DeleteBehavior.Restrict); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
