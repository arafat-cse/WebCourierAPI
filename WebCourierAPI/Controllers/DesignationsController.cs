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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebCourierAPI.Controllers
{
    [EnableCors("Policy1")]
    [AuthAttribute("", "Designations")]
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationsController : ControllerBase
    {
        // GET: api/Designations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Designation>>> GetDesignations()
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            return await _context.Designations.ToListAsync();
        }

        // GET: api/Designations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Designation>> GetDesignation(int id)
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            var designation = await _context.Designations.FindAsync(id);

            if (designation == null)
            {
                return NotFound();
            }

            return designation;
        }

        // PUT: api/Designations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDesignation(int id, Designation designation)
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            if (id != designation.DesignationId)
            {
                return BadRequest("Mismatched Designation ID.");
            }

            var existingDesignation = await _context.Designations.FindAsync(id);
            if (existingDesignation == null)
            {
                return NotFound("Designation not found.");
            }
            var token = Request.Headers["Token"].FirstOrDefault();
            var user = AuthenticationHelper.ValidateToken(token);

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }
            existingDesignation.Title = designation.Title;
            existingDesignation.CreateBy = user.UserName;
            existingDesignation.CreateDate = DateTime.UtcNow;

            _context.Entry(designation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DesignationExists(id))
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
        // POST: api/Designations
        [HttpPost]
        public async Task<ActionResult<Designation>> PostDesignation(Designation designation)
        {
            WebCorierApiContext _context = new WebCorierApiContext();

            if (designation == null || string.IsNullOrEmpty(designation.Title))
            {
                return BadRequest("designation is required");
            }

            var token = Request.Headers["Token"].FirstOrDefault();
            var user = AuthenticationHelper.ValidateToken(token);

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            designation.CreateBy = user.UserName;
            designation.CreateDate = DateTime.UtcNow;

            _context.Designations.Add(designation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDesignation", new { id = designation.DesignationId }, designation);
        }

        // DELETE: api/Designations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            var designation = await _context.Designations.FindAsync(id);
            if (designation == null)
            {
                return NotFound();
            }

            _context.Designations.Remove(designation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DesignationExists(int id)
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            return _context.Designations.Any(e => e.DesignationId == id);
        }
    }
}
