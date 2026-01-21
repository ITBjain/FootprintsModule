using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PegasusFootprintsApi.Data;
using PegasusFootprintsApi.Models;

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
        public async Task<ActionResult<IEnumerable<Template>>> GetTemplates()
        {
            return await _context.Templates.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Template>> GetTemplate(string id)
        {
            var template = await _context.Templates.FindAsync(id);

            if (template == null) return NotFound();

            return template;
        }
    }
}