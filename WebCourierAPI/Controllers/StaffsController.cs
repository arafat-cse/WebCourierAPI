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
    [AuthAttribute("", "Staffs")]
    [Route("api/[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private readonly WebCorierApiContext _context;

        public StaffsController(WebCorierApiContext context)
        {
            _context = context;
        }

        // GET: api/Staffs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Staff>>> GetStaffs()
        {
            return await _context.Staffs.ToListAsync();
        }

        // GET: api/Staffs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Staff>> GetStaff(int id)
        {
            var staff = await _context.Staffs.FindAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return staff;
        }

        // PUT: api/Staffs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaff(int id, Staff staff)
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            if (id != staff.StaffId)
            {
                return BadRequest("Mismatched staff ID.");
            }


            var staffexists = await _context.Staffs.FindAsync(id);
            if (staffexists == null)
            {
                return NotFound("staff not found.");
            }
            var token = Request.Headers["Token"].FirstOrDefault();
            var user = AuthenticationHelper.ValidateToken(token);

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }
            staffexists.StaffName = staff.StaffName;
            staffexists.CreateBy = user.UserName;
            staffexists.CreateDate = DateTime.UtcNow;

            //existingParcelType.ParcelTypeName = parcelType.ParcelTypeName;
            //existingParcelType.UpdateBy = parcelType.UpdateBy;
            //existingParcelType.UpdateDate = DateTime.UtcNow;
            staffexists.IsActive = staffexists.IsActive;

            _context.Entry(staffexists).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(id))
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

        // POST: api/Staffs
        [HttpPost]
        public async Task<ActionResult<Staff>> PostStaff([FromBody] Staff staff)
        {
            WebCorierApiContext _context = new WebCorierApiContext();

            if (staff == null || string.IsNullOrEmpty(staff.StaffName))
            {
                return BadRequest("staff name is required.");
            }

            var token = Request.Headers["Token"].FirstOrDefault();
            var user = AuthenticationHelper.ValidateToken(token);

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            staff.CreateBy = user.UserName;
            staff.CreateDate = DateTime.UtcNow;

            _context.Staffs.Add(staff);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStaff), new { id = staff.StaffId }, staff);
        }

            // DELETE: api/Staffs/5
            [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await _context.Staffs.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            _context.Staffs.Remove(staff);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StaffExists(int id)
        {
            return _context.Staffs.Any(e => e.StaffId == id);
        }
    }
}
