using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCourierAPI.Models;

namespace WebCourierAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParcelsController : ControllerBase
    {
        //private readonly WebCorierApiContext _context;

        //public ParcelsController(WebCorierApiContext context)
        //{
        //    _context = context;
        //}
        //CommanResponse
        private readonly CommanResponse cp = new CommanResponse();
        // GET: api/Parcels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parcel>>> GetParcels()
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            return await _context.Parcels.ToListAsync();
        }

        // GET: api/Parcels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parcel>> GetParcel(int id)
        {
            WebCorierApiContext _context = new WebCorierApiContext();

            //var parcel = await _context.Parcels.FindAsync(id);

            //if (parcel == null)
            //{
            //    return NotFound();
            //}

            //return parcel;

            try
            {
                var parcel = await _context.Parcels.FindAsync(id);

                if (parcel==null)
                {
                    cp.status = false;
                    cp.message = "No branches found.";
                    cp.content = null;
                    return Ok(cp);
                }

                cp.status = true;
                cp.message = "Branches retrieved successfully.";
                cp.content = parcel;
                return Ok(cp);
            }
            catch (Exception ex)
            {
                cp.status = false;
                cp.message = "Error occurred while retrieving branches.";
                cp.errorMessage = ex.Message;
                cp.content = null;
                return StatusCode(500, cp);
            }
        }

        // PUT: api/Parcels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParcel(int id, Parcel parcel)
        {
            WebCorierApiContext _context = new WebCorierApiContext();

            if (id != parcel.ParcelId)
            {
                return BadRequest("Mismatched Company ID.");
            }
            var existingParcel = await _context.Parcels.FindAsync(id);
            if (existingParcel == null)
            {
                return NotFound("Company not found.");
            }
            var token = Request.Headers["Token"].FirstOrDefault();
            var user = AuthenticationHelper.ValidateToken(token);

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }
           
            existingParcel.CreateBy = user.UserName;
            existingParcel.CreateDate = DateTime.UtcNow;

            //existingParcelType.ParcelTypeName = parcelType.ParcelTypeName;
            //existingParcelType.UpdateBy = parcelType.UpdateBy;
            //existingParcelType.UpdateDate = DateTime.UtcNow;
            existingParcel.IsActive = parcel.IsActive;
            _context.Entry(parcel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParcelExists(id))
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

        // POST: api/Parcels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Parcel>> PostParcel(Parcel parcel)
        {
            WebCorierApiContext _context = new WebCorierApiContext();

            //_context.Parcels.Add(parcel);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetParcel", new { id = parcel.ParcelId }, parcel);


            if (parcel == null)
            {
                return BadRequest("Parcel type name is required.");
            }

            var token = Request.Headers["Token"].FirstOrDefault();
            var user = AuthenticationHelper.ValidateToken(token);

            if (user == null)
            {
                return Unauthorized("Invalid or expired token.");
            }

            parcel.CreateBy = user.UserName;
            parcel.CreateDate = DateTime.UtcNow;


            _context.Parcels.Add(parcel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParcel", new { id = parcel.ParcelId }, parcel);

        }

        // DELETE: api/Parcels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParcel(int id)
        {
            WebCorierApiContext _context = new WebCorierApiContext();

            var parcel = await _context.Parcels.FindAsync(id);
            if (parcel == null)
            {
                return NotFound();
            }

            _context.Parcels.Remove(parcel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParcelExists(int id)
        {
            WebCorierApiContext _context = new WebCorierApiContext();

            return _context.Parcels.Any(e => e.ParcelId == id);
        }
    }
}
