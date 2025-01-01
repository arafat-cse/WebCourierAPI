using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCourierAPI.Attributes;
using WebCourierAPI.Models;

namespace WebCourierAPI.Controllers
{
    [EnableCors("Policy1")]
    [AuthAttribute("", "Companies")]
    [Route("api/[controller]")]
    [ApiController]
    public class VansController : ControllerBase
    {
        private readonly WebCorierApiContext _context;

        public VansController(WebCorierApiContext context)
        {
            _context = context;
        }

        // GET: api/Vans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Van>>> GetVans()
        {
            return await _context.Vans.ToListAsync();
        }

        // GET: api/Vans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Van>> GetVan(int id)
        {
            var van = await _context.Vans.FindAsync(id);

            if (van == null)
            {
                return NotFound();
            }

            return van;
        }

        // PUT: api/Vans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVan(int id, Van van)
        {
            if (id != van.VanId)
            {
                return BadRequest();
            }

            _context.Entry(van).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Vans
        [HttpPost]
        public async Task<ActionResult<Van>> PostVan(Van van)
        {
            _context.Vans.Add(van);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVan", new { id = van.VanId }, van);
        }

        // DELETE: api/Vans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVan(int id)
        {
            var van = await _context.Vans.FindAsync(id);
            if (van == null)
            {
                return NotFound();
            }

            _context.Vans.Remove(van);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool VanExists(int id)
        {
            return _context.Vans.Any(e => e.VanId == id);
        }
    }
}
