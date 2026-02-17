using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PegasusFootprintsApi.Data;
using PegasusFootprintsApi.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PegasusFootprintsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        private readonly FootprintsContext _context;

        public TemplatesController(FootprintsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTemplates()
        {
            var templates = await _context.Templates.ToListAsync();
            return Ok(templates.Select(t => MapToApiResponse(t)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTemplate(string id)
        {
            var template = await _context.Templates.FindAsync(id);
            if (template == null) return NotFound();
            return Ok(MapToApiResponse(template));
        }

        private object MapToApiResponse(Template t)
        {
            JsonNode? slotsNode = null;
            int width = 1080;
            int height = 1920;

            if (t.ConfigJson != null)
            {
                try 
                {
                    // JsonDocument has RootElement
                    var rootNode = JsonNode.Parse(t.ConfigJson.RootElement.GetRawText());
                    slotsNode = rootNode?["slots"];
                    
                    if (rootNode?["canvas_width"] != null) width = (int)rootNode["canvas_width"];
                    if (rootNode?["canvas_height"] != null) height = (int)rootNode["canvas_height"];
                }
                catch { }
            }

            return new
            {
                templateId = t.TemplateId,
                title = t.Title,
                category = t.Category,
                thumbnailUrl = t.ThumbnailUrl,
                configJson = new
                {
                    base_width = width,
                    base_height = height,
                    pages = new[]
                    {
                        new
                        {
                            page_num = 1,
                            bg_url = t.BaseImageUrl ?? "",
                            overlay_url = t.OverlayImageUrl ?? "",
                            slots = slotsNode ?? new JsonArray()
                        }
                    }
                }
            };
        }
    }
}