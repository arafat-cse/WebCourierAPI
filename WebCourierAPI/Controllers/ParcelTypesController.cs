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
    public class ParcelTypesController : ControllerBase
    {
        //private readonly WebCorierApiContext _context;

        //public ParcelTypesController(WebCorierApiContext context)
        //{
        //    _context = context;
        //}

        // GET: api/ParcelTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParcelType>>> GetParcelTypes()
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            return await _context.ParcelTypes.ToListAsync();
        }

        // GET: api/ParcelTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParcelType>> GetParcelType(int id)
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            var parcelType = await _context.ParcelTypes.FindAsync(id);

            if (parcelType == null)
            {
                return NotFound();
            }

            return parcelType;
        }

        // PUT: api/ParcelTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParcelType(int id, ParcelType parcelType)
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            if (id != parcelType.ParcelTypeId)
            {
                return BadRequest();
            }

            _context.Entry(parcelType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParcelTypeExists(id))
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

        // POST: api/ParcelTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParcelType>> PostParcelType(ParcelType parcelType)
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            _context.ParcelTypes.Add(parcelType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParcelType", new { id = parcelType.ParcelTypeId }, parcelType);
        }

        // DELETE: api/ParcelTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParcelType(int id)
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            var parcelType = await _context.ParcelTypes.FindAsync(id);
            if (parcelType == null)
            {
                return NotFound();
            }

            _context.ParcelTypes.Remove(parcelType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParcelTypeExists(int id)
        {
            WebCorierApiContext _context = new WebCorierApiContext();
            return _context.ParcelTypes.Any(e => e.ParcelTypeId == id);
        }
    }
}
