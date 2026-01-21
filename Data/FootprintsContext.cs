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
            modelBuilder.Entity<Template>().ToTable("templates");

            // CRITICAL FIX: Add a Value Converter for the JSON column.
            // This prevents the "No suitable constructor" error.
            modelBuilder.Entity<Template>()
                .Property(t => t.ConfigJson)
                .HasConversion(
                    // How to save to DB: Convert JsonElement to String
                    v => v.HasValue ? v.Value.ToString() : null,
                    // How to read from DB: Parse String back to JsonElement
                    // We use Clone() to ensure the element persists safely.
                    v => v != null ? JsonDocument.Parse(v, default).RootElement.Clone() : (JsonElement?)null
                );
        }
    }
}