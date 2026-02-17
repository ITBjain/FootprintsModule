using Microsoft.EntityFrameworkCore;
using PegasusFootprintsApi.Models;
using System.Text.Json;

namespace PegasusFootprintsApi.Data
{
    public class FootprintsContext : DbContext
    {
        public FootprintsContext(DbContextOptions<FootprintsContext> options) : base(options) { }

        public DbSet<Template> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // 1. Table Mapping
            modelBuilder.Entity<Template>().ToTable("frame_templates");

            // 2. Column Mapping (Crucial for fixing "Unknown column")
            modelBuilder.Entity<Template>().Property(t => t.TemplateId).HasColumnName("id");
            modelBuilder.Entity<Template>().Property(t => t.Title).HasColumnName("title");
            modelBuilder.Entity<Template>().Property(t => t.Category).HasColumnName("category");
            modelBuilder.Entity<Template>().Property(t => t.ThumbnailUrl).HasColumnName("thumbnail_url");
            modelBuilder.Entity<Template>().Property(t => t.BaseImageUrl).HasColumnName("bg_layer_url");
            modelBuilder.Entity<Template>().Property(t => t.OverlayImageUrl).HasColumnName("frame_layer_url");

            // 3. JSON Conversion
            modelBuilder.Entity<Template>()
                .Property(t => t.ConfigJson)
                .HasColumnName("layout_config")
                .HasConversion(
                    // To DB
                    v => v != null ? v.RootElement.GetRawText() : null,
                    // From DB
                    v => v != null ? JsonDocument.Parse(v, default) : null
                );
        }
    }
}

// using Microsoft.EntityFrameworkCore;
// using PegasusFootprintsApi.Models;
// using System.Text.Json;

// namespace PegasusFootprintsApi.Data
// {
//     public class FootprintsContext : DbContext
//     {
//         public FootprintsContext(DbContextOptions<FootprintsContext> options) : base(options) { }

//         public DbSet<Template> Templates { get; set; }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);
//             modelBuilder.Entity<Template>().ToTable("frame_templates");

//             // CRITICAL FIX: Add a Value Converter for the JSON column.
//             // This prevents the "No suitable constructor" error.
//             modelBuilder.Entity<Template>()
//                 .Property(t => t.ConfigJson)
//                 .HasConversion(
//                     // How to save to DB: Convert JsonElement to String
//                     v => v.HasValue ? v.Value.ToString() : null,
//                     // How to read from DB: Parse String back to JsonElement
//                     // We use Clone() to ensure the element persists safely.
//                     v => v != null ? JsonDocument.Parse(v, default).RootElement.Clone() : (JsonElement?)null
//                 );
//         }
//     }
// }