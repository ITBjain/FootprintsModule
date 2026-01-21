using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json; // Required for JsonElement

namespace PegasusFootprintsApi.Models
{
    [Table("templates")]
    public class Template
    {
        [Key]
        [Column("template_id")]
        public string TemplateId { get; set; } = string.Empty;

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("category")]
        public string Category { get; set; } = string.Empty;

        [Column("thumbnail_url")]
        public string? ThumbnailUrl { get; set; }

        [Column("base_image_url")]
        public string? BaseImageUrl { get; set; }

        [Column("overlay_image_url")]
        public string? OverlayImageUrl { get; set; }

        [Column("default_page_count")]
        public int DefaultPageCount { get; set; }

        // CRITICAL FIX:
        // Changed from 'JsonDocument' (which causes the crash) to 'JsonElement'.
        // JsonElement is supported by EF Core 8/9+ for mapping JSON columns.
        [Column("config_json", TypeName = "json")]
        public JsonElement? ConfigJson { get; set; }
    }
}